import json, random

all_patients = {}
queue = []
dismissed = []
admitted = []
dead = []

time = 0
patients_to_generate = 60
end_game = 60*12
patients_per_hour = 5


def reset_time():
  time = 0
  #queue = geneate_patient_queue(patients_to_generate) 
  patients = [{'name':'fred'}, {'name':'ben'}, {'name': 'lisa'}]
  for i, patient in enumerate(patients): 
    patient['id'] = str(i)
    all_patients[str(i)] = patient
    queue.append(patient)

def get_next_patient():  
  global time
  
  if time < end_game and len(queue) > 0:
    time += 12
    patient_id = queue.pop()
    patient = all_patients[patiend_id]

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
  queue.insert(patients_per_hour*2, all_patients[id]) # defer 2 hours

def admit_patient(id):
  admitted.append(all_patients[id])

def dismiss_patient(id):
  dismissed.append(all_patients[id])

def game_state():
  state = {}
  state['state'] = 'in play' if (time < end_game or len(queue) == 0) else 'ended'
  state['time'] = time
  state['total_time'] = end_game
  state['admitted'] = len(admitted)
  state['dismissed'] = len(dismissed)
  
  if time >= end_game or len(queue) == 0:
    state['dead'] = len(dead)   
    state['score'] = patients_to_generate - len(data)
   
  return state
