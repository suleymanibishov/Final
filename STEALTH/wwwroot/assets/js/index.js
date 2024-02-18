document.querySelectorAll('#Questions .questions .question .plus').forEach(element => {

    element.addEventListener('click', (e) => {
        var id = element.parentNode.getAttribute("data-id")
        const el = document.querySelectorAll(`#Questions .questions [data-id=\"${id}\"]`);
        el[1].setAttribute("style", "height: auto; display: block;");

        el[0].querySelector('.minus').style.display = 'block';
        el[0].querySelector('.plus').style.display = 'none';

    })

});
document.querySelectorAll('#Questions .questions .question .minus').forEach(element => {

    element.addEventListener('click', (e) => {
        var id = element.parentNode.getAttribute("data-id")
        const el = document.querySelectorAll(`#Questions .questions [data-id=\"${id}\"]`);
        el[1].setAttribute("style", "height: 0px; display: none;");

        el[0].querySelector('.plus').style.display = 'block';
        el[0].querySelector('.minus').style.display = 'none';

    })

});

document.querySelectorAll('.basket-btn').forEach(
    element => {
        element.addEventListener('click', () => {
            fetch(`/basket/addBasket/${element.getAttribute('data-id')}`)
        })
    }
)

//--------------------------------------------------------------------

// fetch('/', {
//     method: 'GET',
//     headers: {
//         'Content-Type': 'application/json'
//     }
// })
// .then()

// GET isteği için fetch kullanımı
// fetch('http://localhost:8080/', {
//   method: 'GET',
//   mode: 'no-cors'
// })
//   .then(response => {
//     // İstek başarılıysa buraya gelir
//     // if (!response.ok) {
//     //   throw new Error('Network response was not ok');
//     // }
//     console.log(response);
//     console.log(response.text());
//     console.log(response.json());

//     return response.json(); // Veya response.json() kullanabilirsiniz, veri tipine göre
//   })
//   .then(data => {
//     // Veriyi kullanın
//     console.log(data);
//   })
//   .catch(error => {
//     // Hata durumu
//     console.error('There was a problem with the fetch operation:', error);
//   });

//   fetch('http://localhost:8080/', {
//     method: 'GET',
//     mode: 'no-cors'
//   })
//   .then(response => response.json())
//   .then(data => {
//     console.log(data); // Beklenen format: { "data": [...] }
//   })
//   .catch(error => {
//     console.error('There was a problem with the fetch operation:', error);
//   });

//----------================================------------------------------

// fetch("http://localhost:8080/")
// .then(res => res.json())
// .then((res) => {
//     console.log(res.data);
//     let html='';
//     res.data.forEach(e=>{
//         console.log(e);
//         html+=`
//         <div class="product-card">
//                     <div class="img">
//                         <img class="cart-img" src="./assets/img/${e.img}">
//                         <a>
//                             <img class="cart-icon" src="./assets/img/shopping-bag.svg">
//                         </a>
//                     </div>
//                     <div class="content">
//                         <h3 class="title">${e.title}</h3>
//                         <p class="price">${e.price}$</p>
//                     </div>
//                     <div>
//                         <img class="star" src="./assets/img/Star 1.svg">
//                         <img class="star" src="./assets/img/Star 1.svg">
//                         <img class="star" src="./assets/img/Star 1.svg">
//                         <img class="star" src="./assets/img/Star 1.svg">
//                         <img class="star" src="./assets/img/Star 1.svg">
//                     </div>
//                 </div>
//         `;
//     });
//     document.querySelector('#Products .featured-products').innerHTML=(html);
// })


// fetch("http://localhost:8080/")

//----------------===================================----------------------




// Example POST method implementation:
// async function postData(url = "", data = {}) {
//     // Default options are marked with *
//     const response = await fetch(url, {
//         method: "POST", // *GET, POST, PUT, DELETE, etc.
//         mode: "cors", // no-cors, *cors, same-origin
//         cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
//         credentials: "same-origin", // include, *same-origin, omit
//         headers: {
//             "Content-Type": "application/json",
//             // 'Content-Type': 'application/x-www-form-urlencoded',
//         },
//         redirect: "follow", // manual, *follow, error
//         referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
//         body: JSON.stringify(data), // body data type must match "Content-Type" header
//     });
//     return response.json(); // parses JSON response into native JavaScript objects
// }

// postData("https://localhost:8080/", {
//     test: 'salam'
// }).then((data) => {
//     console.log(data); // JSON data parsed by `data.json()` call
// });