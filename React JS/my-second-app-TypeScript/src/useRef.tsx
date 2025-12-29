import { useRef } from "react";

function FocusInput() {
  const inputRef = useRef<HTMLInputElement>(null);

  //   function checkNull(inputRef: any | null) {
  //     if (inputRef == null) inputRef.current.value = "Elizabeth Liao";
  //   }

  return (
    <>
      <h2>This is my FocusInput file</h2>
      <input ref={inputRef} onFocus={() => inputRef.current?.select()} />
    </>
  );
}

export default FocusInput;
