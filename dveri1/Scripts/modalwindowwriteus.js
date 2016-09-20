//==============================================��������� ������� �� ������ �������� ���========================================================
$(document).ready(function () {
$('a.butwritemain').click(function (event) { // �o��� ���� �o ������ � ������� butcallmain
    event.preventDefault(); // ������a�� ��a��a����� �o�� �������a
    $('#overlay').fadeIn(400, //��a�a�a ��a��o �o�a���a�� ������ �o��o���
        function () { //�o��� ���o������ ��������� a���a���
            $('#modal_form_writeus')
                .css('display', 'block') // ����a�� � �o�a���o�o o��a display: none;
                .animate({ opacity: 1, top: '50%' }, 200); // ��a��o ����a����� ��o��a��o��� o��o�������o �o �����a���� ����
        });
});
$.ajax({//������������ ajax �������
    type: 'POST',
    url: '/VhodnyeDveri/WriteToUsModal',
    dataType: 'json',//����� ���� � ������� json
    success: function (data) {
        //������� ����� �������� ��������� � �������
        $('#idWrite').replaceAll(data);
    },
})


/* �a������ �o�a���o�o o��a, ��� ���a�� �o �� �a�o� �o � o��a��o� �o����� */
$('#modal_close, #overlay, .butcancel').click(function (event) {   // �o��� ���� �o �������� ��� �o��o���
    event.preventDefault();
    $('#modal_form_writeus')
        .animate({ opacity: 0, top: '45%' }, 200,  // ��a��o ������ ��o��a��o��� �a 0 � o��o�������o ����a�� o��o �����
            function () { // �o��� a���a���
                $(this).css('display', 'none'); // ���a�� ��� display: none;
                $('#overlay').fadeOut(400); // �����a�� �o��o���
            }
        );
});
})