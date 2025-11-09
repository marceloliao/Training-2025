import os
import re

# -- Regex match example from Copilot
# pattern = r"\d+"  # Example pattern to match one or more digits
# text = "The price is 100 dollars and the discount is 20 dollars."

# matches = re.findall(pattern, text)
# print("Matches found:", matches)

# print(f"The number of matches: {len(matches)} ")
# print(f"Matches found: {matches[0]}, {matches[1]}")


# -- To loop through all directories from Copilot
# def loop_through_subdirectories(path):
#     for root, dirs, files in os.walk(path):
#         print(f"Current directory: {root}")
#         print(f"Subdirectories: {dirs}")
#         print(f"Files: {files}")
#         print("-" * 40)


# # Example usage
# path = r"C:\Users\maliao\Documents\PS Projects\110 HP TMS requests - Script to delete ETMA users, plus TCR request"
# loop_through_subdirectories(path)


file = r"C:\Users\maliao\Documents\PS Projects\110 HP TMS requests - Script to delete ETMA users, plus TCR request\2025-01-06 Script result from Dilip\ETMA_USER_DELETE-OUTPUT_ITG.txt"

# Need to open in UTF-8 encoding
f = open(file, encoding="utf8")

lines = f.readlines()

# Declare a dictionary
sql_tables = []
all_occurrences = []

re_pattern = 'table "dbo.tbl.*"'

for line in lines:

    matches = re.findall(re_pattern, line)

    for match in matches:

        # Insert them all to all_occurences list so we can extract the count
        all_occurrences.append(match)

        # Insert to the list if it's not in there already
        if sql_tables.count(match) == 0:
            sql_tables.append(match)
            print(match)

f.close()

print(sql_tables)

result_file = file.replace("ETMA_USER_DELETE-OUTPUT_ITG", "Result")

with open(result_file, 'w') as file:
    for item in sql_tables:
        file.write(f"{item} - {all_occurrences.count(item)}\n")
    # file.writelines(sql_tables) This method writes all items in one line
