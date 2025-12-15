let x: unknown = "4";
console.log((<string>x).length);
console.log((x as string).length);

export {};