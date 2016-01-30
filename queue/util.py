import json, random
from patient_gen import generate_patient

all_patients = {}
beds = []
queue = []
dismissed = []
admitted = []
dead = []
time = 0

patients_to_generate = 60
end_game = 60*12
patients_per_hour = 5
bed_limit = 10
bed_decay = 36


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
  for i in range(patients_to_generate): 
    patient = dict(generate_patient())
    patient['id'] = str(i)
    all_patients[str(i)] = patient
    queue.append(patient['id'])

def get_next_patient():  
  global time
  global queue
  global beds
  global admitted
  global dismissed
  global all_patients
  
  if time < end_game and len(queue) > 0:
    time += 12
    patient_id = queue.pop()
    patient = all_patients[patient_id]
  
    if time % bed_decay == 0 and len(beds) > 0:
      beds.pop()

    if 'arrival_time' not in patient:
      patient['arrival_time'] = time

    if 'ailment_deadline' in patient and patient['ailment_deadline'] > -1:
      if time > (patient['arrival_time'] + patient['ailment_deadline']):
        dead.append(patient['id'])
        return get_next_patient()

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
  state['state'] = 'in play' if (time < end_game and len(queue) > 0) else 'ended'
  state['time'] = time
  state['total_time'] = end_game
  state['admitted'] = len(admitted)
  state['dismissed'] = len(dismissed)
  state['in_queue'] = len(queue)
  state['total_beds'] = bed_limit
  state['used_beds'] = len(beds)
  
  if time >= end_game or len(queue) == 0:
    state['dead'] = len(dead)   
    state['score'] = patients_to_generate - len(dead)
   
  return state
