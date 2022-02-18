var table = $('#items-table');

$('.item-input').on('input', changeAddButtonState);

$('#add-item-row').on('click', function()
{
    table.append(rowTemplate);
    $('#add-item-row').addClass('d-none')
    $('#add-item-row').removeClass('d-block')
    $('.item-input').on('input', changeAddButtonState);
});

function changeAddButtonState()
{
    let enable = true;
    $('.item-input').each(function(index, element){
        console.log(enable)
        if($(element).val() === ""){
            enable = false;
        }
        console.log(enable)
    });

    if(enable){
        $('#add-item-row').removeClass('d-none')
        $('#add-item-row').addClass('d-block')
    }
    else{
        $('#add-item-row').removeClass('d-block')
        $('#add-item-row').addClass('d-none')
    }
}

var rowTemplate  = `<tr>
<td class="w-25">
  <input placeholder="Macbook Pro" type="text" class="item-input input-wrapper-noborder p-2 text-light" />
</td>
<td class="w-25">
  <input placeholder="27" type="number" class="item-input w-75 input-wrapper-noborder p-2 text-light" />
</td>
<td class="w-25">
  <input placeholder="1235" type="number" class="item-input w-75 input-wrapper-noborder p-2 text-light" />
</td>
<td class="w-25">
  <h4 class="text-gray">$ 1253</h4>
</td>
</tr>`;