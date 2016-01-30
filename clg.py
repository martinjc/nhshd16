from pyfiglet import Figlet


class Game:

	def __init__(self):
		f = Figlet()
		print(f.renderText('Patient, Please'))

g = Game()
