import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import Greeting from "./Greeting.tsx";
import { SaveButton, NameInput } from "./Events.tsx";
import MyForm from "./States.tsx";
import FocusInput from "./useRef.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <Greeting name="Marcelo" age={30} />
    <SaveButton />
    <NameInput />
    <MyForm />
    <FocusInput />
  </StrictMode>
);
