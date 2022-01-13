import sys, getopt
import os
from valvebsp import Bsp
pt_attr = [['origin','1 1 1'],
           ['vscripts' , 'vs_eventlistener.nut'],
           ['Template01','vs.eventlistener'] ,
           ['spawnflags' , '2'] ,
           ['classname','point_template'],
           ['hammerid' , '9876543']]
le_attr = [['origin','1 1 2'] ,
           ['TeamNum' , '-1'] ,
           ['targetname','vs.eventlistener'] ,
           ['IsEnabled' , '1'] ,
           ['FetchEventData' , '0'] ,
           ['classname','logic_eventlistener'],
           ['hammerid' , '9876575']]
def writeBsp(s,o):
    extension = os.path.splitext(s)[1]
    print(extension)
    if extension.lower() == ".bsp":
        bsp = Bsp(s)
        entities = bsp[0]
        ent_c = len(entities)
        entities.insert(ent_c + 1,pt_attr)
        entities.insert(ent_c + 2,le_attr)
        bsp.save(o)
        print("Successfully created BSP!")
    
def main(argv):
   inputfile = ''
   outputfile = ''
   try:
      opts, args = getopt.getopt(argv,"hi:o:",["ifile=","ofile="])
   except getopt.GetoptError:
        print('test.py -i <inputfile> -o <outputfile>')
        sys.exit(2)
   for opt, arg in opts:
      if opt == '-h':
         print('test.py -i <inputfile> -o <outputfile>')
         sys.exit()
      elif opt in ("-i", "--ifile"):
         inputfile = arg
      elif opt in ("-o", "--ofile"):
         outputfile = arg
   writeBsp(inputfile,outputfile)
   

if __name__ == "__main__":
   main(sys.argv[1:])