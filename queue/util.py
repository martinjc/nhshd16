import json, random, threading, uuid
from patient_gen import generate_patient
from time import sleep

defer_time = 120
end_game = 60 * 12
bed_limit = 10
bed_decay = 30

patient_generation = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1]
tick_time = 5
tick_rate = 5

class Game:
  def __init__(self):
    self.all_patients = {}
    self.beds = []
    self.queue = []
    self.dismissed = []
    self.deferred = []
    self.admitted = []
    self.dead = []
    self.time = 0

game = Game()

def create_patients(n):
  global game

  for i in range(2): 
    patient = dict(generate_patient())
    patient['id'] = str(uuid.uuid4())
    game.all_patients[patient['id']] = patient
    game.queue.append(patient['id'])

def reset_time():
  global game 

  game = Game()
  create_patients(2)
    
  def timer():
    while not is_game_ended():
      increment_time()
      sleep(tick_time)
  thr = threading.Thread(target=timer)
  thr.start()

def increment_time():
  global game
  
  game.time += tick_rate
  create_patients(random.choice(patient_generation))

  # check for dead patients
  for patient in game.all_patients:
    if 'ailment_deadline' in patient and patient['ailment_deadline'] > -1 and 'arrival_time' in patient:
      if game.time > (patient['arrival_time'] + patient['ailment_deadline']) and patient['id'] not in game.admitted:
        game.dead.append(patient['id'])
        game.queue.remove(patient['id'])

  # check patients leaving beds
  for id in game.beds :
    if game.time >= game.all_patients[id]['arrival_time'] + bed_decay:
      game.beds.remove(id)


def get_next_patient():  
  global game
 
  if is_game_ended():
    return {'game_ended': True}

  for patient_id in game.deferred:
    print patient_id
    if game.all_patients[patient_id]['deferred_until'] <= game.time:
      patient_id = game.deferred.pop()
      return game.all_patients[patient_id]
 
  if len(game.queue) > 0:
    patient_id = game.queue.pop()
    patient = game.all_patients[patient_id]
    if 'arrival_time' not in patient:
      patient['arrival_time'] = game.time
      patient['deferred'] = False
    return patient
  else:
    return {}

def defer_patient(id):
  global game
 
  game.all_patients[id]['deferred_until'] = game.time + defer_time
  game.all_patients[id]['deferred'] = True
  game.deferred.append(id)
  return {'OK': True}

def admit_patient(id):
  global game 

  if len(game.beds) < bed_limit:
    gaeme.beds.append(id)
    game.admitted.append(id)

  if len(game.beds) >= bed_limit:
    return {'beds_full': True}
  else:
    return {'beds_full': False}

def dismiss_patient(id):
  global game 

  game.dismissed.append(id)
  return {'OK': True}

def game_state():
  global game

  state = {}
  state['time'] = game.time
  state['total_time'] = end_game
  state['admitted'] = len(game.admitted)
  state['deferred'] = len(game.deferred)
  state['dismissed'] = len(game.dismissed)
  state['in_queue'] = len(game.queue)
  state['total_beds'] = bed_limit
  state['used_beds'] = len(game.beds)
  
  if is_game_ended():
    state['state'] = 'ended'
    state['dead'] = len(game.dead)   
  else:
    state['state'] = 'in play'
   
  return state

def is_game_ended():
  global game

  return game.time >= end_game
