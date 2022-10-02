var currentUserInfo;
// alert(document.cookie);
async function loginFetch(loginUrl, formData)
{
    await fetch(loginUrl,
        {
            method: 'POST',
            body: formData
        }).
        then(res => res.json()).
        then(data => 
            {
               currentUserInfo = data;
               //console.log(currentUserInfo);
            }
        ).
        catch(error => console.log(error)).
        finally(() => {
            //console.log(currentUserInfo);
            //return currentUserInfo;
        });

        

        document.cookie = `username=${currentUserInfo[1]}; expires=Wed, 5 Oct 2022 17:00:00 UTF; path=/`;
        document.cookie = `token=${currentUserInfo[0]}; expires=Wed, 5 Oct 2022 17:00:00 UTF; path=/`;


}

function login()
{
    
    
    const loginUrl = 'https://localhost:44389/api/account/login'
    const loginForm = document.getElementById('login-form');
    loginForm.addEventListener('submit', async e =>
    {
        e.preventDefault();
        const formData = new FormData(loginForm);

        await loginFetch(loginUrl, formData);
        
    });

    
}

/*function for login done.*/


async function loadIntoTable(url, table)
{
    const tableBody = table.querySelector("tbody");
    const response = await fetch(url);
    const data = await response.json();
    
    tableBody.innerHTML = "";

    for (var i = 0, size = data.length; i < size; i++)
    {
        var item = data[i];

        const rowElement = document.createElement("tr");
        
        
        const cells = [item['reviewBookName'], item['reviewTitle'], item['reviewDate'].slice(0, 10), ''];
        const cellImage = item['imageSrc'];
        
        //console.log(cells);
        const imgElement = document.createElement("img");
        const revButton = document.createElement("button");
        const revLink = document.createElement("a");

        revButton.className = "signup-submit";
        revLink.href = `ViewReview.html?${item['id']}`;
        revLink.text = "Go to review"; 
        revButton.appendChild(revLink);
        imgElement.src = cellImage;

        

        for (cellText of cells)
        {
            
            const cellElement = document.createElement('td');
            cellElement.textContent = cellText;
            cellElement.appendChild(imgElement);
            cellElement.appendChild(revButton);

            rowElement.appendChild(cellElement);
            //rowElement.appendChild(imgElement);
        }
        

        tableBody.appendChild(rowElement);
    }
}

loadIntoTable("https://localhost:44389/api/reviews/get-all", document.getElementById('reviews-table'));


async function CheckLoggedIn()
{
    const token = document.cookie.split('token=')[1];
    //alert(token);
    const checkUrl = 'https://localhost:44389/api/reviews/add-review';
    const response = await fetch(checkUrl, {method: 'POST',
    headers: {
        Authorization: `Bearer ${token}`
    }});
    if (response.status == 401)
    {
        window.location.replace('Login.html');
    }
    
    

}

async function LoadReview()
{
    const id = window.location.href.split('?')[1];
    const url = `https://localhost:44389/api/reviews/get-id/${id}`;

    const bookTitle = document.getElementById('book-title');
    const reviewTitle = document.getElementById('review-title');
    const reviewBody = document.getElementById('review-body');
    const coverImg = document.getElementById('book-cover');

    const response = await fetch(url);
    const data = await response.json();
    
    console.log(data);
    coverImg.src = data['imageSrc'];
    bookTitle.textContent = data['reviewBookName'];
    reviewTitle.textContent = data['reviewTitle'];
    reviewBody.textContent = data['reviewBody'];

    
}

