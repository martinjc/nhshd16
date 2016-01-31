import json, random, threading, uuid
from patient_gen import generate_patient
from time import sleep

all_patients = {}
beds = []
queue = []
dismissed = []
admitted = []
dead = []
time = 0

patients_to_generate = 70
end_game = 60*12
patients_per_hour = 5
bed_limit = 10
bed_decay = 36
tick_time = 3


def reset_time():
  global time
  global queue
  global beds
  global admitted
  global dismissed
  global all_patients

  time = 0

  all_patients = {}
  beds = []
  queue = []
  admitted = []
  dismissed = []
  dead = []
  for i in range(2): 
    patient = dict(generate_patient())
    patient['id'] = str(uuid.uuid4())
    all_patients[patient['id']] = patient
    queue.append(patient['id'])
  
  def timer():
    while not is_game_ended():
      increment_time()
      sleep(tick_time)
  thr = threading.Thread(target=timer)
  thr.start()

def increment_time():
  global time
  global queue
  global beds
  global admitted
  global dismissed
  global all_patients
  
  time += 5

  # check for dead patients
  for patient in all_patients:
    if 'ailment_deadline' in patient and patient['ailment_deadline'] > -1 and 'arrival_time' in patient:
      if time > (patient['arrival_time'] + patient['ailment_deadline']) and patient['id'] not in admitted:
        dead.append(patient['id'])
        queue.remove(patient['id'])

  # check patients leaving beds
  for id in beds :
    if time >= all_patients[id]['arrival_time'] + bed_decay:
      beds.remove(id)


def get_next_patient():  
  global time
  global queue
  global beds
  global admitted
  global dismissed
  global all_patients
  
  if time < end_game and len(queue) > 0:
    patient_id = queue.pop()
    patient = all_patients[patient_id]
    if 'arrival_time' not in patient:
      patient['arrival_time'] = time
    return patient
  else:
    return {'end_game': True}

def defer_patient(id):
  global queue

  try:
    queue.insert(patients_per_hour*2, id) # defer 2 hours
  except:
    pass
  return {'OK': True}

def admit_patient(id):
  global beds
  global admitted

  if len(beds) < bed_limit:
    beds.append(id)
    admitted.append(id)

  if len(beds) >= bed_limit:
    return {'beds_full': True}
  else:
    return {'beds_full': False}

def dismiss_patient(id):
  global dismissed

  dismissed.append(id)
  return {'OK': True}

def game_state():
  state = {}
  state['time'] = time
  state['total_time'] = end_game
  state['admitted'] = len(admitted)
  state['dismissed'] = len(dismissed)
  state['in_queue'] = len(queue)
  state['total_beds'] = bed_limit
  state['used_beds'] = len(beds)
  
  if is_game_end():
    state['state'] = 'ended'
    state['dead'] = len(dead)   
    state['score'] = patients_to_generate - len(dead)
  else:
    state['state'] = 'in play'
   
  return state

def is_game_ended():
  global time
  global end_game

  return time >= end_game
  
