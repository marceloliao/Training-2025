import csv
import sys


def main():

    # Check if arguments are passed
    if len(sys.argv) > 0:

        # Print the arguments
        for i, arg in enumerate(sys.argv[1:], start=1):
            print(f"Argument {i}: {arg}")

            sourceFile = sys.argv[1]
            resultFile = sourceFile.replace(".csv", "_result.csv")

            print(f"sourceFile: {sourceFile}")
            print(f"targetFile: {resultFile}")

            # sourceFile = r"C:\Users\maliao\Downloads\Task_Status_01.csv"
            # resultFile = r"C:\Users\maliao\Downloads\Task_Status_01_result.csv"

            rows = []

            # Open the CSV file
            with open(sourceFile, encoding="utf8", mode='r') as file:

                # Create a CSV reader object
                csv_reader = csv.reader(file)

                # Get the header row (if your CSV file has headers)
                headers = next(csv_reader)

                print(f"header line {headers}")

                rows.append(headers)

                previous_project_Id = headers[3]
                previous_source_file_Id = headers[7]
                previous_language_pair = headers[9]

                # Iterate through the rows in the CSV file
                for row in csv_reader:

                    # Access individual columns by index
                    project_Id = row[3]
                    source_file_Id = row[7]
                    language_pair = row[9]

                    if project_Id != previous_project_Id or source_file_Id != previous_source_file_Id or language_pair != previous_language_pair:
                        rows.append(row)
                        previous_project_Id = project_Id
                        previous_source_file_Id = source_file_Id
                        previous_language_pair = language_pair

                    # # Print the columns
                    # print(f"Project Id: {project_Id}, Source File Id: {source_file_Id}")

            with open(resultFile, 'w', newline='', encoding="utf8") as file:
                writer = csv.writer(file)
                writer.writerows(rows)

    else:
        print("No arguments passed.")


if __name__ == "__main__":
    main()
