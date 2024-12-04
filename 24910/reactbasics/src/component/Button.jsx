import "../App.css"

const Button = ({label, styling}) => {
  return (
    <div>
        <button style={styling}>{label}</button>
        </div>
  )
}

export default Button




