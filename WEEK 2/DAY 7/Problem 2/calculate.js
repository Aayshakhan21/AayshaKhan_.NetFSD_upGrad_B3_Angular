import { calculateTotal } from "./cart.js";

const cart = [
  { name: "Laptop", price: 50000, quantity: 1 },
  { name: "Mouse", price: 500, quantity: 2 },
  { name: "Keyboard", price: 1500, quantity: 1 },
];

cart.map((item) => {
  console.log(`${item.name} - ₹${item.price} x ${item.quantity}`);
});

const totalAmount = calculateTotal(cart);
console.log(`Total Cart Value: ₹${totalAmount}`);
