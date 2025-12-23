// Input change
export function NameInput() {
  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    console.log(e.target.value);
  }
  return <input onChange={handleChange} />;
}

// Button click
export function SaveButton() {
  const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    alert(`You clicked on the Save button!`);
    event.preventDefault();
  };
  // function handleClick(e: React.MouseEvent<HTMLButtonElement>) {
  //   e.preventDefault();
  // }

  return <button onClick={handleClick}>Save</button>;
}
