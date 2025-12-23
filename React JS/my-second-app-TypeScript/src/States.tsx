import { useState } from "react";

function MyForm() {
  const [count, setCount] = useState<number>(0);
  const [status, setStatus] = useState<"idle" | "loading" | "error">("idle");

  type User = { id: number; name: string };
  const [user, setUser] = useState<User | null>({ id: 15, name: "George" });

  const handleClick = () => {
    setCount((count) => count + 1);
    setStatus((status) => (status = "loading"));
    setUser((user) => (user = { id: 20, name: "Mary" }));
  };

  return (
    <>
      <h1>Using TypeScript in React</h1>
      <p>Here's the count of click: {count}</p>
      <p>Here's the current state: {status}</p>
      <p>
        Here's the current user: {user?.name}, this user has Id equal to{" "}
        {user?.id}.
      </p>

      <button onClick={handleClick}>count is {count}</button>
    </>
  );
}

export default MyForm;
