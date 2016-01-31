from flask import Flask
from util import *
import json

app = Flask(__name__)

@app.route('/game/start')
def start_game():
  reset_time()
  return json.dumps({'success': True})

@app.route('/game/state')
def get_state():
  return json.dumps(game_state())

@app.route('/patients/next')
def next_patient():
  return json.dumps(get_next_patient())

@app.route('/patients/<id>/<action>', methods=['POST'])
def edit(id, action):
  if action == 'dismiss':
    return json.dumps(dismiss_patient(id))
  elif action == 'defer':
    return json.dumps(defer_patient(id))
  elif action == 'admit':
    return json.dumps(admit_patient(id))
  return {'Error': True}

@app.route('/handbook/complete')
def handbook() :
  return json.dumps(get_handbook())

@app.route('/handbook/by_patient/<id>')
def handbook_by_patient(id) :
  return json.dumps(get_handbook(id))

if __name__ == '__main__':
    app.debug = True
    app.run(host='0.0.0.0', port=5000)
