interface Person {
  age: number;
  name: string;
}

function printPersonProperties(person: Person, key: keyof Person) {
  console.log(`Printing person with the key "${key}": ${person[key]}`);
}

let person: Person = { name: "Max", age: 45 };
printPersonProperties(person, "age");

type StringMap = { [key: string]: unknown };

function createStringPair(property: keyof StringMap, value: string): StringMap {
  return { [property]: value };
}

console.log(JSON.stringify(createStringPair("greeting", "hello")));
