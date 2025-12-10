type Animal = { name: string };
type Bear = Animal & { honey: boolean };
const bear: Bear = { name: "Winnie", honey: true };

console.log(bear);

type Status = "success" | "error";  // either success or error, cannot be anything else
let response: Status = "success";

console.log(response);
