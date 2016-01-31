import json, random, threading, uuid, datetime, numpy
from patient_gen import generate_patient, generate_handbook
from time import sleep
import numpy as np

defer_time = 120
end_game = 45 * 1
bed_limit = 10
bed_decay = 60

average_new_patients = 0.5  # e.g., 0.3 means 1/3 patient every `tick_time`
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
    self.real_time = datetime.datetime.now().replace(hour = 8, minute = 0, second = 0)
  def is_ended(self):
    return self.time >= end_game

game = Game()

def create_patients(n):
  global game

  for i in range(n):
    patient = dict(generate_patient())
    patient['id'] = str(uuid.uuid4())
    game.all_patients[patient['id']] = patient
    game.queue.append(patient['id'])
    patient['arrival_time'] = game.time
    patient['status'] = 'alive'

def reset_time():
  global game

  game = Game()
  create_patients(2)

  def timer():
    while not game.is_ended():
      increment_time()
      sleep(tick_time)
  thr = threading.Thread(target=timer)
  thr.start()

def increment_time():
  global game

  game.time += tick_rate
  game.real_time += datetime.timedelta(minutes = tick_rate)

  num_create = np.random.poisson(lam=average_new_patients)
  create_patients(num_create)

  # check for dead patients
  for patient_id in game.all_patients:
    patient = game.all_patients[patient_id]
    if patient['ailment_deadline'] > -1 and game.time > (patient['arrival_time'] + patient['ailment_deadline']) and patient['id'] not in game.admitted and patient['id'] not in game.dead:
        game.dead.append(patient['id'])
        if patient['id'] in game.deferred:
          game.deferred.remove(patient['id'])
        if patient['id'] in game.queue:
          game.queue.remove(patient['id'])
        game.all_patients[patient['id']]['status'] = 'dead'

  # check patients leaving beds
  for id in game.beds :
    if game.time >= game.all_patients[id]['admission_time'] + bed_decay:
      game.beds.remove(id)


def get_next_patient():
  global game

  if game.is_ended():
    return {'game_ended': True}

  for patient_id in game.deferred:
    if game.all_patients[patient_id]['deferred_until'] <= game.time:
      game.deferred.remove(patient_id)
      return game.all_patients[patient_id]

  if len(game.queue) > 0:
    patient_id = game.queue.pop()
    patient = game.all_patients[patient_id]
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
    game.beds.append(id)
    game.admitted.append(id)
    game.all_patients[id]['admission_time'] = game.time

  if len(game.beds) >= bed_limit:
    return {'beds_full': True}
  else:
    return {'beds_full': False}

def dismiss_patient(id):
  global game

  game.dismissed.append(id)
  return {'OK': True}

def get_handbook(patient_id=None):
  handbook = generate_handbook()
  if patient_id :
    patient = all_patients[patient_id]
    return filter(lambda el: el['symptom'] in patient['symptoms'], handbook)
  return { 'symptoms' : handbook }

def get_time_display(time_to_show):
  hour = time_to_show.hour
  minute = time_to_show.minute
  hour = '0' + str(hour) if hour < 10 else str(hour)
  minute = '0' + str(minute) if minute < 10 else str(minute)
  return hour + ':' + minute

def game_state():
  global game

  state = {}
  state['time'] = game.time
  state['real_time'] = get_time_display(game.real_time)
  state['total_time'] = end_game
  state['admitted'] = len(game.admitted)
  state['deferred'] = len(game.deferred)
  state['dismissed'] = len(game.dismissed)
  state['in_queue'] = len(game.queue)
  state['total_beds'] = bed_limit
  state['used_beds'] = len(game.beds)

  if game.is_ended():
    state['state'] = 'ended'
    state['dead'] = []
    state['num_dead'] = len(game.dead)
    score = 0
    score -= 2 * len(game.dead)
    for patient_id in game.admitted:
      if game.all_patients[patient_id]['ailment_deadline'] > -1:
        score -= 1
    for patient_id in game.dead:
      state['dead'].append(game.all_patients[patient_id])
    state['score'] = score

  else:
    state['state'] = 'in play'

  return state
