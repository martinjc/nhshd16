from flask import Flask
from util import *
import json

app = Flask(__name__)

@app.route('/game/start')
def start_game():
  reset_time()   

@app.route('/patient/next')
def get_next_patient():
  return json.dumps(generate_new_patient())

@app.route('/patient/<id>/<action>', methods=['POST'])
def edit(id, action):
  if action == 'dismiss':
    dismiss_patient(id)
  elif action == 'defer':
    defer_patient(id)
  elif action == 'admin':
    accept_patient(id) 

if __name__ == '__main__':
    app.debug = True
    app.run(host='0.0.0.0', port=5000)
