from pyfiglet import Figlet
import requests

class Game:
	ROOT = 'http://localhost:5000'
	URLS = {
		'START': ROOT + '/game/start',
		'STATE': ROOT + '/game/state'
	}
	# GET_PATIENT_URL = ROOT + '/patient/next'

	def __init__(self):
		self.f = Figlet()
		self.render('Patient, Please')

	def start(self):
		r = requests.get(self.URLS['START'])
		self.render(r.text)

	def state(self):
		r = requests.get(self.URLS['STATE'])
		#TODO: handle game starting and stopping

	def stop(self):
		self.render('Game Over')

	def render(self, text):
		print(self.f.renderText(text))

class Patient:

	ROOT = 'http://localhost:5000'
	URLS = {
		'GET': ROOT + '/game/start',
		'DEFER': ROOT + '/game/state',
		'DISMISS': ROOT + '',
		'ADMIT': ROOT + ''
	}

	def __init__(self, game, ID, name, age, gender, photoPath, symptoms):
		self.game = game
		self.ID = ID
		self.name = name
		self.age = age
		self.gender = gender
		self.photoPath = photoPath
		self.symptoms = symptoms

	def dismiss(self):
		self.game.render('Dismissing Patient %d' % self.ID)

	def defer(self):
		self.game.render('Deferring Patient %d' % self.ID)

	def admit(self):
		self.game.render('Admitting Patient %d' % self.ID)

g = Game()
g.start()
g.stop()
