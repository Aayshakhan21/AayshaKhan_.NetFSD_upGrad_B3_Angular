class Vehicle {
  constructor(brand, speed) {
    this.brand = brand;
    this.speed = speed;
  }

  showInfo() {
    console.log("Brand: " + this.brand);
    console.log("Speed: " +  this.speed + "km/h");
  }
}

class Car extends Vehicle {
  constructor(brand, speed, fuelType) {
    super(brand, speed);
    this.fuelType = fuelType;
  }

  showDetails() {
    console.log("Fuel Type:", this.fuelType);
  }
}

const car1 = new Car("Maruti", 120, "Petrol");
car1.showInfo();
car1.showDetails();
