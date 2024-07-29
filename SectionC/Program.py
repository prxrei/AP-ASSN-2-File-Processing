from datetime import datetime
from functools import reduce

# Single statement for calculation using filter and map
total_sales = reduce(
    lambda acc, sale: acc + sale,
    map(
        lambda line: float(line.split('|')[11]) * int(line.split('|')[8]) * 1.35,
        filter(
            lambda line: datetime.strptime(line.split('|')[3], '%d/%m/%Y').year >= 2020 and line.split('|')[7] in ['B', 'C'],
            open("../ProdMasterlistB.txt", 'r').readlines()
        )
    ),
    0.0  # Initial value as float
)

# Print the total sales using a single map statement
list(map(lambda total: print(f"Total Sales After Discount and Sales Rise: ${total:.2f}"), [total_sales]))
