const baseUrl = 'https://localhost:7251/api/player';
const authToken = sessionStorage.getItem("authToken");

function loadJewelry() {
    $.ajax({
        url: baseUrl,
        type: 'GET',
        headers: {
            'Authorization': 'Bearer ' + authToken
        },
        success: function (data) {
            displayJewelryList(data);
            console.log(data)
        }
    });
}

function displayJewelryList(data) {
    let html = '<table><tr><th>ID</th><th>FullName</th><th>ClubName</th><th>Achievements</th><th>Nomination</th><th>Birthday</th><th>PlayerExperiences</th><th>Actions</th></tr>';
    /*doi may cai nay thanh entity*/
    data.forEach(function (item) {
        html += `<tr>
            <td>${item.footballPlayerId}</td> 
            <td>${item.fullName}</td>
            <td>${item.clubName}</td>
            <td>${item.achievements}</td>
            <td>${item.nomination}</td>
            <td>${item.birthday}</td>
            <td>${item.playerExperiences}</td>
            <td>
                <button onclick="showEditForm('${item.footballPlayerId}')">Edit</button>
                <button onclick="deleteJewelry('${item.footballPlayerId}')">Delete</button>
            </td>
        </tr>`;
    });
    html += '</table>';
    $('#jewelryList').html(html);
}

function loadCategories() {
    $.ajax({
        url: baseUrl + '/categories',
        type: 'GET',
        headers: {
            'Authorization': 'Bearer ' + authToken
        },
        success: function (data) {
            let options = '';
            /*Nhớ đổi mấy cái này thành styleId*/
            data.forEach(function (club) {
                options += `<option value="${club.footballClubId}">${club.clubName}</option>`;
            });
            $('#newFootballClubId').html(options);
            $('#editFootballClubId').html(options);
            /*Nhớ đổi mấy cái này thành styleId*/
        }
    });
}

function showAddForm() {
    $('#addForm').show();
    $('#editForm').hide();
}

function showEditForm(id) {
    $.ajax({
        url: baseUrl + '/' + id,
        type: 'GET',
        headers: {
            'Authorization': 'Bearer ' + authToken
        },
        /*Nhớ đổi mấy cái này thành entity match với bên View*/
        success: function (data) {
            $('#editFootballPlayerId').val(data.footballPlayerId);
            $('#editFullName').val(data.fullName);
            $('#editAchievements').val(data.achievements);
            $('#editBirthday').val(data.birthday);
            $('#editNomination').val(data.nomination);
            $('#editPlayerExperiences').val(data.playerExperiences);
            $('#editFootballClubId').val(data.footballClubId);
            $('#editForm').show();
            $('#addForm').hide();
        }
        /*Nhớ đổi mấy cái này thành entity match với bên View*/
    });
}

function addJewelry() {
    /*Nhớ đổi mấy cái này thành entity match với bên View*/
    const newJewelry = {
        footballPlayerId: $('#newFootballPlayerId').val(),
        fullName: $('#newFullName').val(),
        achievements: $('#newAchievements').val(),
        birthday: $('#newBirthday').val(),
        nomination: $('#newNomination').val(),
        playerExperiences: $('#newPlayerExperiences').val(),
        footballClubId: $('#newFootballClubId').val()
    };
    /*Nhớ đổi mấy cái này thành entity match với bên View*/
    $.ajax({
        url: baseUrl,
        type: 'POST',
        headers: {
            'Authorization': 'Bearer ' + authToken,
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(newJewelry),
        success: function () {
            loadJewelry();
            $('#addForm').hide();
        }
    });
}

function updateJewelry() {
    /*Nhớ đổi mấy cái này thành entity match với bên View*/
    const updatedJewelry = {
        footballPlayerId: $('#editFootballPlayerId').val(),
        fullName: $('#editFullName').val(),
        achievements: $('#editAchievements').val(),
        birthday: $('#editBirthday').val(),
        nomination: $('#editNomination').val(),
        playerExperiences: $('#editPlayerExperiences').val(),
        footballClubId: $('#editFootballClubId').val()
    };
    /*Nhớ đổi mấy cái này thành entity match với bên View*/

    $.ajax({
        url: baseUrl,
        type: 'PUT',
        headers: {
            'Authorization': 'Bearer ' + authToken,
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(updatedJewelry),
        success: function () {
            loadJewelry();
            $('#editForm').hide();
        }
    });
}

function deleteJewelry(id) {
    if (confirm('Are you sure you want to delete this jewelry?')) {
        $.ajax({
            url: baseUrl + '/' + id,
            type: 'DELETE',
            headers: {
                'Authorization': 'Bearer ' + authToken
            },
            success: function () {
                loadJewelry();
            }
        });
    }
}

function searchJewelry() {
    /*Nhớ đổi mấy cái này thành entity match với bên View Search*/
    const paintingAuthor = $('#searchNomination').val();
    const paintingAchive = $('#searchAchievements').val();
    let searchUrl = baseUrl + '/search?';

    // Only append parameters if they have a value
    if (paintingAchive) {
        searchUrl += 'Achievements=' + encodeURIComponent(paintingAchive) + '&';
    }
    if (paintingAuthor) {
        searchUrl += 'Nomination=' + encodeURIComponent(paintingAuthor);
    }
    /*Nhớ đổi mấy cái này thành entity match với bên View*/

    // Remove trailing '&' if present
    searchUrl = searchUrl.replace(/&$/, '');

    $.ajax({
        url: searchUrl,
        type: 'GET',
        headers: {
            'Authorization': 'Bearer ' + authToken
        },
        success: function (data) {
            displayJewelryList(data);
        }
    });
}

$(document).ready(function () {
    loadJewelry();
    loadCategories();
});

