import json, random

all_patients = {}
queue = []
dismissed = []
admitted = []

time = 0
end_game = 60
patients_per_hour = 5

def reset_time():
  time = 0
  #queue = geneate_patient_queue() 
  patients = [{'name':'fred'}, {'name':'ben'}, {'name': 'lisa'}]
  for i, patient in enumerate(patients): 
    patient['id'] = str(i)
    all_patients[str(i)] = patient
    queue.append(patient)

def get_next_patient():  
  global time
  
  if time < 60:
    time += 1 # 12 minutes
    patient = queue.pop()
    print patient
    return patient
    #return queue.pop()

def defer_patient(id):
  queue.insert(patients_per_hour*2, all_patients[id])

def admit_patient(id):
  admitted.append(all_patients[id])

def dismiss_patient(id):
  dismissed.append(all_patients[id])

def game_state():
  state = {}
  state['state'] = 'in play' if time < end_game else 'ended'
  state['time'] = time
  state['total_time'] = end_game
  return state
