import json, random

all_patients = {}

queue = []
dismissed = []
admitted = []

ailments_file = open('ailments.json', 'r')
ailments = json.loads(ailments_file)
time = 0
end_game = 60
patients_per_hour = 5

def reset_time():
  time = 0
  queue = geneate_patient_queue() 
  for i, patient in enumerate(queue): 
    all_patients[i] = patient

def get_next_patient():  
  if time == 60:
    return end_game()
  time += 1 # 12 minutes
  return queue.pop()

def defer_patient(id):
  queue.insert(patients_per_hour*2, all_patients[id])

def admit_patient(id):
  admitted.append(all_patients[id])

def dismiss_patient(id):
  dismissed.append(all_patients[id])

def end_game():
  pass
