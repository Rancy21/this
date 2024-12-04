import { useState } from 'react'

const Products = () => {
    const [items, setItems] = useState([
        {

            id: 1,
            name: "nike"
        },
        {
            id: 2,
            name: "Adidas"
        }
    ])
    return (
        <div>
            <h1>Total Products: {items.length}</h1>

            {items.length > 0 ? 
            items.map((item) => {
                return <div key={item.id}>{item.name}</div>
            }) : 
            <p>No Items</p>}
        </div>
    )
}

export default Products