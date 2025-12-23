// Required

interface Car {
  make: string;
  model: string;
  year?: number;
}

let myCar: Required<Car> = {
  make: "Ford",
  model: "Focus",
  year: 2024,
};

console.log(myCar.year);

// Record, Record<string, number> is equivalent to { [key: string]: number }

const nameAgeMap: Record<string, number> = {
  Alice: 21,
  Bob: 25,
};

console.log(nameAgeMap["Bob"]);

type nameAgeMapType = { [key: string]: number };
const nameAgeMap2: nameAgeMapType = {};
nameAgeMap2["Maria"] = 24;
nameAgeMap2["Marcus"] = 45;
console.log(nameAgeMap2);

export {};
