interface Rectangle {
    height: number,
    width: number
};

const rectangle: Rectangle = {
    height: 40,
    width: 40
};

console.log(rectangle);

interface Mammal { name: string };
interface Mammal { age: number };

const dog: Mammal = { name: "Fido", age: 10 };
console.log(dog);
