class Student {
  constructor(name, rollNumber, marks) {
    this.name = name;
    this.rollNumber = rollNumber;
    this.marks = marks;
  }

  getDetails() {
    console.log("Name:", this.name);
    console.log("Roll Number:", this.rollNumber);
    console.log("Marks:", this.marks);
  }

  getGrade() {
    if (this.marks >= 90) {
      return "A";
    } else if (this.marks >= 75) {
      return "B";
    } else if (this.marks >= 50) {
      return "C";
    } else {
      return "Fail";
    }
  }
}

const student1 = new Student("Aaysha", 101, 92);
const student2 = new Student("Rahul", 102, 68);

student1.getDetails();
console.log("Grade:", student1.getGrade());
student2.getDetails();
console.log("Grade:", student2.getGrade());
