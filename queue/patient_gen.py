import os.path
import csv
import json
import pandas as pd
from pprint import pprint
from functools32 import lru_cache
import random
import collections
import numpy as np


@lru_cache(None)
def load_people():
    fp = "./dat_people/faces.csv"
    df = pd.read_csv(fp, index_col='filename')
    df = df.dropna(how='any')
    return df


@lru_cache(None)
def load_ailments():
    fp = './dat_ailments/ailments.json'
    with open(fp) as f:
        dat = json.load(f)
        return dat

@lru_cache(None)
def load_symptoms():
    fp = './dat_symptoms/symptoms.json'
    with open(fp) as f:
        dat = json.load(f)
        return dat

@lru_cache(None)
def load_names():
    fp = './dat_people/names.txt'
    df = pd.read_csv(fp, sep=', ')
    df = df.dropna(how='any')
    return df


age2range = {
    'Y': [18, 30],
    'M': [30, 55],
    'O': [55, 70]
}


def generate_skeleton():
    photo_row = load_people().reset_index().sample(n=1)
    photo_fname = tuple(photo_row.loc[:,'filename'])[0]
    photo_fp = photo_fname

    sex = photo_row.loc[:,'sex']
    sex = tuple(sex)[0].lower()
    assert sex in ['m', 'f']

    df_names = load_names()
    name = df_names[df_names['sex'] == sex].sample(n=1)['name']
    name = tuple(name)[0]

    age_id = tuple(photo_row.loc[:,'age'])[0]
    age_min, age_max = age2range[age_id]
    age = random.randint(age_min, age_max)

    dat = collections.OrderedDict(
        [('name', name), ('sex', sex), ('age', age),
        ('photo_fpath', photo_fp), ('photo_fname', photo_fname)])
    return dat


def generate_ailment():
    ails = load_ailments()
    weights = np.array([ail['ailment_prob'] for ail in ails], np.float64)
    weights = weights / weights.sum()
        # dumb! not satistical! wheeeeee
    ail = np.random.choice(ails, p=weights)
    return ail


def roulette(prob):
    return random.random() <= prob


def select_symptoms(symps_seq):
    symptoms = set()
    while len(symptoms) == 0:
        for symp_dat in symps_seq:
            if roulette(symp_dat['prob']):
                symptoms.add(symp_dat['name'])
    return sorted(symptoms)


def generate_patient():
    dat = generate_skeleton()
    ail = generate_ailment()
    dat['ailment_name'] = ail['ailment_name']
    dat['ailment_deadline'] = ail['ailment_deadline']
    dat['symptoms'] = select_symptoms(ail['symptoms'])
    return dat


def generate_handbook():
  handbook = []
  symptoms = load_symptoms()
  ailments = load_ailments()

  for symptom in symptoms :
    symptom_name = symptom['name']
    causes = []
    for ailment in ailments :
      if symptom['name'] in map(lambda obj: obj['name'], ailment['symptoms']) :
        causes.append(ailment['ailment_name'])

    handbook.append({'name' : symptom_name, 'causes' : causes})
  return handbook


if __name__ == "__main__":
    for _ in xrange(10):
        pat = generate_patient()
        pprint(dict(pat))

