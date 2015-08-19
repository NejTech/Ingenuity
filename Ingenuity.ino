#include <SPI.h>
#include <AccelStepper.h>

AccelStepper stepper1(5, 6, 7, 8, 9); // right stepper
AccelStepper stepper2(5, 2, 3, 4, 5); // left stepper

volatile byte command;
volatile bool takeAction;

void setup() {
  Serial.begin(9600);
  
  pinMode(MISO, OUTPUT);
  pinMode(MOSI, INPUT);
  // turn on SPI in slave mode
  SPCR |= _BV(SPE);
  SPI.attachInterrupt();
  
  stepper1.setMaxSpeed(1500.0);
  stepper1.setAcceleration(1200.0);
  stepper2.setMaxSpeed(1500.0);
  stepper2.setAcceleration(1200.0);
}

void loop() {
  stepper1.run();
  stepper2.run();
  
  /*
  if ( command & (1 << 0) )
    Serial.println("forward");
  if ( command & (1 << 1) )
    Serial.println("backward");
  if ( command & (1 << 2) )
    Serial.println("right");
  if ( command & (1 << 3) )
    Serial.println("left");
  if ( command & (1 << 4) )
    Serial.println("tilt up");
  if ( command & (1 << 5) )
    Serial.println("tilt down");
  if ( command & (1 << 6) )
    Serial.println("pan right");
  if ( command & (1 << 7) )
    Serial.println("pan left");
    */
  
  
  
  // Command byte structure
  // forward,backward,right,left,tilt up,tilt down,pan right,pan left
  
  if (takeAction) {
    if (command & (1 << 7)) { // forward +
      if (command & (1 << 5)) {        // right
        stepper1.move(-768);
        stepper2.move(2048);
      }
      else if (command & (1 << 4)) {   // left
        stepper1.move(-2048);
        stepper2.move(768);
      }
      else {                           // straight
        stepper1.move(-2048);
        stepper2.move(2048);
      }
	  
      takeAction = false;
    }
    
    else if (command & (1 << 6)) { // backward +
      if (command & (1 << 5)) {         // right
        stepper1.move(768);
        stepper2.move(-2048);
      }
      else if (command & (1 << 4)) {    // left
        stepper1.move(2048);
        stepper2.move(-768);
      }
      else {                            // straight
        stepper1.move(2048);
        stepper2.move(-2048);
      }
	  
      takeAction = false;
    }
    
    else if (command & (1 << 5)) { // right only
      stepper1.move(2048);
      stepper2.move(2048);
      
      takeAction = false;
    }
    
    else if (command & (1 << 4)) { // left only
      stepper1.move(-2048);
      stepper2.move(-2048);
      
      takeAction = false;
    }
  }
}

// SPI interrupt routine
ISR (SPI_STC_vect)
{
  command = SPDR;
  takeAction = true;
}
