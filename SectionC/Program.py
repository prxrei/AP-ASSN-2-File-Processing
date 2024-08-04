from datetime import datetime
from functools import reduce

try:
    # Open the file and read lines
    with open("ProdMasterlistB.txt", 'r') as file:
        lines = file.readlines()
        
except FileNotFoundError:
    print("Error: File not found.")
    total_sales = 0.0

except Exception as err:
    print(f"Unexpected error: {err}")
    total_sales = 0.0

else:
    # Single statement for calculation using filter and map
    total_sales = reduce(
        lambda acc, sale: acc + sale,
        map(
            lambda line: float(line.split('|')[11]) * int(line.split('|')[8]) * 1.35,
            filter(
                lambda line: datetime.strptime(line.split('|')[3], '%d/%m/%Y').year >= 2020 and line.split('|')[7] in ['B', 'C'],
                lines
            )
        ),
        0.0  # Initial value as float
    )

# Print the total sales using a single map statement
list(map(lambda total: print(f"\nTotal Sales for Products of Discount Types B & C after Discount and Sales Rise: ${total:.2f}"), [total_sales]))
