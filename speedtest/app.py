# create 1024kb txt file with 1s and 0s

import os

def create_file():
    with open('speedtest.txt', 'w') as f:
        for i in range(1024 * 1024):
            if i % 2 == 0:
                f.write('0')
            else:
                f.write('1')

if __name__ == '__main__':
    create_file()

