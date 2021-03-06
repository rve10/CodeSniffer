import os
import matplotlib.pyplot as plt
from matplotlib.widgets import Slider, Button, RadioButtons
import numpy as np

from scipy.stats import norm
from numpy import genfromtxt

def plot_data(filename, columns, descr):
    my_data = genfromtxt(filename, delimiter=',', comments='(', skip_header=False, names=True)

    for name in columns:
        fig = plt.figure(figsize=(6,2))

        data = my_data[name].tolist()

        data.sort()

        std = np.std(data) 
        print("{0}: {1}".format(name, std))
        mean = np.mean(data)

        outlierBorder = std * 8

        print("outlierBorder: {}".format(outlierBorder))

        plt.axvline(outlierBorder, color="red", linestyle="dashed", marker="8")
        plt.title("{} - {}".format(name, descr))
        
        plt.boxplot(data, vert=False)

        os.makedirs(os.path.dirname(os.path.realpath(__file__)) + "\\trainingDataResults\\{}".format(os.path.basename(filename)[:-4]), exist_ok=True)

        plt.savefig(os.path.dirname(os.path.realpath(__file__)) + "\\trainingDataResults\\{}\\{}.png".format(os.path.basename(filename)[:-4],name))
        plt.close(fig)

def plot_method_data():
    columns = ["LOC", "CYCLO", "ATFD", "FDP", "LAA", "MAXNESTING", "NOAV"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\TrainingsData\\MethodTrainingSet_2319_17022018.csv"
    plot_data(filename, columns, "Method Level - GanttProject")


def plot_class_data():
    columns = ["LOC", "TCC", "WMC", "ATFD"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\TrainingsData\\ClassTrainingSet_2319_17022018.csv"
    plot_data(filename, columns, "Class Level - GanttProject")

def plot_verification_method_data():
    columns = ["LOC", "CYCLO", "ATFD", "FDP", "LAA", "MAXNESTING", "NOAV"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\VerificationData\\MethodTrainingSet_1357_18022018.csv"
    plot_data(filename, columns, "Method Level - JUnit")

def plot_verification_class_data():
    columns = ["LOC", "TCC", "WMC", "ATFD"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\VerificationData\\ClassTrainingSet_1357_18022018.csv"
    plot_data(filename, columns, "Class Level - Junit")


plot_class_data()
plot_method_data()

plot_verification_class_data()
plot_verification_method_data()
 

