const marks = [75, 80, 65, 90, 55];

const totalMarks = marks.reduce((total, mark) => total + mark, 0);

const averageMarks = totalMarks / marks.length;

const result = averageMarks >= 40 ? "PASS" : "FAIL";

console.log(`Student Marks: ${marks.join(", ")}`);
console.log(`Total Marks: ${totalMarks}`);
console.log(`Average Marks: ${averageMarks.toFixed(2)}`);
console.log(`Result: ${result}`);