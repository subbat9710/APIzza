
let orderRequest={
        pizzas: [],
        specialtyPizza:[],
        sides:[],
        beverage:[],

        address: {
        homeType: "",
        streetAddress: "",
        apartment: "",
        city: "",
        stateAbbreviation: "",
        zip: "",
        addressID: 0,
        instructions: ""
        },
        customerOrder: {
        orderId: 0,
        orderCost: 0,
        email: "",
        phoneNumber: "",
        orderStatus: "",
        orderTime: "",
        orderType: "",
        addressId: 0
        }
    };
    let pizza ={
        
            pizzaId: 0,
            size: {
            size: 0
            },
            toppings: [
            ],
            crusts: {
            crusts: 0
            },
            sauces: {
            sauces: 0
            },
            pizzaQuantity: 0
            
        
    };
    let sideOrder={
      name:"",
      quantity:"",

    };
    let bevOrder={
      name:"",
      quantity:"",

    };
    let specialtyPizzaOrder={
      name:"",
      quantity:"",

    };
    let toppings4p ={ 
    topping: 0
    };
    export default {
        convertNoahObToTulaOb(array,addy,custo) {
          const incoming = { ...orderRequest };
// adds adress to object
            incoming.address.homeType = addy.homeType
            incoming.address.streetAddress = addy.streetAddress
            incoming.address.apartment = addy.apartment
            incoming.address.city = addy.city
            incoming.address.stateAbbreviation = addy.stateAbbreviation
            incoming.address.zip = addy.zip
            incoming.address.instructions = addy.instructions

// adds costumer to object
        incoming.customerOrder.email = custo.email
        incoming.customerOrder.phoneNumber = custo.phoneNumber
        incoming.customerOrder.orderType = custo.orderType

            array.forEach((item) => {
            // if object type is pizza add properties to pizza then adds pizza to array
            if (item.type == "specialtyPizza"){
              const incomingSpecialtyPizza = { ...specialtyPizzaOrder};
              incomingSpecialtyPizza.name = item.name;
              incomingSpecialtyPizza.quantity = item.quantity;
              incoming.specialtyPizza.push(incomingSpecialtyPizza)
            }
            if (item.type == "Beverage"){
              const incomingBevOrder = { ...bevOrder};
              incomingBevOrder.name = item.name;
              incomingBevOrder.quantity = item.quantity;
              incoming.beverage.push(incomingBevOrder)
            }
            if (item.type == "Side"){
              const incomingSide = { ...sideOrder};
              incomingSide.name = item.name;
              incomingSide.quantity = item.quantity;
              incoming.sides.push(incomingSide)

            }


            if (item.type == "Pizza") {
              const incomingPizza = { ...pizza };
      
              incomingPizza.pizzaQuantity = item.customPizzaCount;
              switch (item.size) {
                case "MD":
                  incomingPizza.size.size = 1;
                  break;
                case "LG":
                  incomingPizza.size.size = 2;
                  break;
                case "XL":
                  incomingPizza.size.size = 3;
                  break;
                default:
                  incomingPizza.size.size = 0;
              }
              switch (item.crust) {
                case "Thin Crust":
                  incomingPizza.crusts.crusts = 1;
                  break;
                default:
                  incomingPizza.crusts.crusts = 0;
              }
      
              switch (item.sauce) {
                case "White":
                  incomingPizza.sauces.sauces = 1;
                  break;
                case "Azure":
                  incomingPizza.sauces.sauces = 2;
                  break;
                default:
                  incomingPizza.sauces.sauces = 0;
              }
      
              if (item.toppings.length > 0) {
                item.toppings.forEach((toppingfromCart) => {
                  let incomingToppings = { ...toppings4p };
                  switch (toppingfromCart) {
                    case "Pepperoni":
                      incomingToppings.topping = 0;
                      break;
                    case "Mushroom":
                      incomingToppings.topping = 1;
                      break;
                    case "Ham":
                      incomingToppings.topping = 2;
                      break;
                    case "Bacon":
                      incomingToppings.topping = 3;
                      break;
                    case "Sausage":
                      incomingToppings.topping = 4;
                      break;
                    case "ExtraCheese":
                      incomingToppings.topping = 5;
                      break;
                    case "Pineapple":
                      incomingToppings.topping = 6;
                      break;
                    case "GoldFlake":
                      incomingToppings.topping = 7;
                      break;
                    case "Spinach":
                      incomingToppings.topping = 8;
                      break;
                    case "Onion":
                      incomingToppings.topping = 9;
                      break;
                    case "Garlic":
                      incomingToppings.topping = 10;
                      break;
                    case "Tomato":
                      incomingToppings.topping = 11;
                      break;
                    case "Green Peppers":
                      incomingToppings.topping = 12;
                      break;
                    case "Banana Peppers":
                      incomingToppings.topping = 13;
                      break;
                    case "Chicken":
                      incomingToppings.topping = 14;
                      break;
                    case "Steak":
                      incomingToppings.topping = 15;
                      break;
                    default:
                      incomingToppings.topping = 1;
                  }
                  incomingPizza.toppings.push(incomingToppings.topping);
                });
              }
              incoming.pizzas.push(incomingPizza);
            }













          });
          return incoming;
        },
      };
      