// Optional Chaining ?.
interface House {
  neighborhood: string;
  yard?: {
    width: number;
    length: number;
    size: number;
  };
}

function printYardSize(house: House) {
  const yardSize = house.yard?.size;
  if (yardSize === undefined) {
    console.log("No yard");
  } else {
    console.log(`Yard is about ${yardSize} square feet.`);
  }
}

let myHouse: House = {
  neighborhood: "Brossard",
};

printYardSize(myHouse);

// Nullish Coalescing
function printMileage(mileage: number | null | undefined) {
  console.log(`Mileage: ${mileage ?? "Not available"}`);
}

printMileage(null);
printMileage(undefined);
printMileage(200);

// Null assertion
function getValue(): string | undefined {
  return "Hello";
}

let value = getValue();
console.log("Value lngth: " + value!.length);

// Array bounds handling
let array: number[] = [1, 2, 3];
let val = array[4];
console.log(val);
