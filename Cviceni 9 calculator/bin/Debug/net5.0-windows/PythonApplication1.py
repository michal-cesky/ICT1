"""import struct

# Convert a large integer to a binary string in little-endian byte order
firstNumber1  = 20000000
secondNumber1 = 30000000
#operand1 = '*'
#bytes(operand1, "utf-8")
#large_value = 100
#packed_value = struct.pack("<i", large_value)

firstNumber = struct.pack("<i", firstNumber1)
secondNumber = struct.pack("<i", secondNumber1)
#operand = struct.pack("<c", operand1)

# Send the binary string as input to the calculator program
# (Assuming the program reads input from standard input)
import subprocess
p = subprocess.Popen(["Cviceni 09 calculator.exe"], stdin=subprocess.PIPE)
#p.communicate(input=packed_value)

p.communicate(input=firstNumber)
p.communicate(input=secondNumber)
#p.communicate(input=operand)"""





