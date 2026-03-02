
export const calculateTotal = (products) => {
  const total = products.reduce(
    (sum, item) => sum + item.price * item.quantity,
    0,
  );

  return total;
};
