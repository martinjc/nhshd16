from flask import Flask

app = Flask(__name__)

@app.route('/patient/next')
def get_next_patient():
  pass

@app.route('/patient/<id>/<action>', methods=['POST'])
def edit(id, action):
  pass

if __name__ == '__main__':
    app.debug = True
    app.run(host='0.0.0.0', port=5000)
