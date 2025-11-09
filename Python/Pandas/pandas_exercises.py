import pandas as pd

mydataset = {
    'cars': ["BMW", "Volvo", "Ford"],
    'passings': [3, 7, 2]
}

myvar = pd.DataFrame(mydataset)

print(pd.__version__)
print(myvar)
print(type(mydataset))

a = [1, 7, 20, 78]

# myvar = pd.Series(a)
# To create custom labels
myvar = pd.Series(a, index=["x", "y", "z", "u"])

# print(myvar)
# print(myvar["z"])

calories = {"day1": 420, "day2": 380, "day3": 390}

# myvar = pd.Series(calories)
# To select only some of keys
# myvar = pd.Series(calories, index=["day1", "day2"])

data = {
    "calories": [420, 380, 390],
    "duration": [50, 40, 45]
}

myvar = pd.DataFrame(data)

print(myvar)
