var currentUserInfo;
// alert(document.cookie);

CheckLoggedIn();

if (window.location.href.includes("Index.html")){
    loadIntoTable("https://localhost:44389/api/reviews/get-all", document.getElementById('reviews-table'));
}

if (window.location.href.includes("UserPage.html")){
    LoadUserTable(document.getElementById('user-reviews'));
}

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

async function SignUp()
{
    const url = 'https://localhost:44389/api/account/signup';
    const signupForm = document.getElementById('signup-form');
    
    signupForm.addEventListener('submit', async e =>
    {
        const signupFormData = new FormData(signupForm);
        await fetch(url, {
            method: 'POST',
            body: signupFormData
        }).
        then(res => res.json()).
        then(data => console.log(data))
        .catch(error => console.log(error));
    });
    

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

async function LoadUserTable(table)
{
    const username = document.cookie.split(';')[0];
    const token = document.cookie.split('token=')[1];

    const url=`https://localhost:44389/api/reviews/get-user-reviews?${username}`;

    const tableBody = table.querySelector("tbody");
    const response = await fetch(url, {
        headers: {
            Authorization: `Bearer ${token}`
        }
    });
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
        const revUpdate = document.createElement("button");
        const updateLink = document.createElement("a");
        const revDelete = document.createElement("button");
        const deleteLink = document.createElement("a");

        revButton.className = "signup-submit";
        revUpdate.className = "signup-submit";
        revDelete.className = "signup-submit";
 
        
        revLink.href = `ViewReview.html?${item['id']}`;
        updateLink.href = `Update.html?${item['id']}`;
        deleteLink.href = `#?${item['id']}`;

        revLink.text = "Go to review"; 
        updateLink.text = "Update review";

        deleteLink.text = "Delete review";
        deleteLink.addEventListener('click', () => {
            Delete(deleteLink.href.split('?')[1]);
        });



        revButton.appendChild(revLink);
        revUpdate.appendChild(updateLink);
        revDelete.appendChild(deleteLink);

        imgElement.src = cellImage;

        

        for (cellText of cells)
        {
            
            const cellElement = document.createElement('td');
            cellElement.textContent = cellText;
            cellElement.appendChild(imgElement);
            cellElement.appendChild(revButton);
            cellElement.appendChild(revUpdate);
            cellElement.appendChild(revDelete);


            rowElement.appendChild(cellElement);
           
        }
        

        tableBody.appendChild(rowElement);
    }


}

function Delete(id)
{
    DeleteReview(id);
    window.location.reload();
}

async function DeleteReview(id)
{
    const token = document.cookie.split('token=')[1];
    const url = `https://localhost:44389/api/reviews/remove-review/${id}`;
    
    await fetch(url, 
        {
            method: 'DELETE',
            headers: {
                Authorization: `Bearer ${token}`
            }

        }).
        then(res => res.json()).
        then(data => console.log(data)).
        catch(error => console.log(error));

}



//Doesn't work unless I do it this way
//Can't have to onloads in an HTML file

async function GetReviewInfo()
{
    await LoadReview();
}

async function UpdateReview()
{
    const id = window.location.href.split('?')[1];
    const Updateurl = `https://localhost:44389/api/reviews/edit-review/${id}`;
    const updateForm = document.getElementById('update-form');
    
    const token = document.cookie.split('token=')[1];
    
    updateForm.addEventListener('submit', async e => {
        e.preventDefault();
        const updateFormData = new FormData(updateForm);
        //console.log(reviewFormData);
        await fetch(Updateurl, {
            method: 'PUT',
            headers: {
                Authorization: `Bearer ${token}`
            },
            body: updateFormData
        }).then(res => res.json()).then(data => console.log(data))
        .catch(error => console.log(error));

    });
    

    

}



function UpdateNavBar()
{
    if (document.cookie.length == 0)
    {
        const nameDisplay = document.getElementById('name-display');
        const logout = document.getElementById('logout');

        logout.style.display = 'none';
        nameDisplay.style.display = 'none';
        return;
    }

    const signup = document.getElementById('signup');
    const login = document.getElementById('login');

    signup.style.display = 'none';
    login.style.display = 'none';
}


async function CheckLoggedIn()
{
    const logcheck = check();
    if (logcheck == null && window.location.href.includes("ReviewForm.html"))
    {
        window.location.replace("Login.html");
    }
    else{
    
        GetUserInfo();
        UpdateNavBar();
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

    bookTitle.value = data['reviewBookName'];
    bookTitle.textContent = data['reviewBookName'];
    reviewTitle.textContent = data['reviewTitle'];
    reviewTitle.value = data['reviewTitle'];
    reviewBody.textContent = data['reviewBody'];

    
}

async function GetUserInfo()
{
    const username = document.cookie.split(';')[0];
    const url = `https://localhost:44389/api/account/get-user?${username}`;

    const response = await fetch(url);
    const userInfo = await response.json();

    const Name = document.getElementById('name-display');
    Name.textContent = userInfo['firstName'] + ' ' + userInfo['lastName'];

}   



function check()
{
    if (document.cookie.length == 0)
    {
        return null;
    }
    
    return document.cookie;
}

async function getReviewImg()
{
    const uploadFile = document.getElementById('file');
    const uploadImg = document.getElementById('book-cover');

    let reader = new FileReader();
    reader.readAsDataURL(uploadFile.files[0]);
    console.log(uploadFile.files[0]);
    reader.onload = () => {
        uploadImg.setAttribute("src", reader.result);
    }


}

async function UploadReview()
{
    const username = document.cookie.split(';')[0];
    const url = `https://localhost:44389/api/reviews/add-review?${username}`;
    const reviewForm = document.getElementById('review-form');

    const token = document.cookie.split('token=')[1];
    
    reviewForm.addEventListener('submit', async e => {
        e.preventDefault();
        const reviewFormData = new FormData(reviewForm);
        //console.log(reviewFormData);
        await fetch(url, {
            method: 'POST',
            headers: {
                Authorization: `Bearer ${token}`
            },
            body: reviewFormData
        })
        .catch(error => console.log(error));

    });
    
}

function logout()
{
    const expiry = new Date("2021-01-1");
    
    let expiryFormatted = expiry.toDateString();
    expiryFormatted += " 12:00:00 UTF";

    const username = document.cookie.split(';')[0];
    const token = document.cookie.split('token=')[1];


    document.cookie = `${username}; expires=${expiryFormatted}; path=/`;
    document.cookie = `token=${token}; expires=${expiryFormatted}; path=/`;

}

function LogoutRefresh()
{
    logout();
    window.location.reload();
}