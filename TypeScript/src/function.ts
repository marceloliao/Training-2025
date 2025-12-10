function getDay(): string {
    return new Date().toTimeString();
}

console.log(getDay());

function divide(dividend: number, divisor: number) {
    return dividend / divisor;
}

console.log(divide(10, 2));

// Named parameters
function divide2({ dividend, divisor }: { dividend: number, divisor: number }) {
    return dividend / divisor;
}

console.log(divide2({ dividend: 15, divisor: 3 }));