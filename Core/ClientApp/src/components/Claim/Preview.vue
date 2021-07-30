<template>
    <div class="container space-contianer" align="center">
       <div class="stepper-wrapper">
            <div class="stepper-item completed">
                <div class="step-counter"><ion-icon name="receipt-outline" style="font-size: 20px; color: white"></ion-icon></div>
                <p style="color: #5c2e91">สร้างคำร้อง</p>
            </div>
            <div class="stepper-item completed">
                <div class="step-counter"><ion-icon name="card-outline" style="font-size: 20px; color: white"></ion-icon></div>
                <p style="color: #5c2e91">บัญชีรับเงิน</p>
            </div>
            <div class="stepper-item completed">
                <div class="step-counter"><ion-icon name="send-outline" style="font-size: 20px; color: white"></ion-icon></div>
                <p style="color: #5c2e91">ส่งคำร้อง</p>
            </div>
        </div>
        <h2 id="header2">รายละเอียดคำร้อง</h2>
        <div id="app" class="container">
           <div class="box-container">
                <p class="mb-0">โรงพยาบาล</p>
                <input type="text" class="h-10 rounded-lg outline-none" placeholder=""/>
                <p class="mb-0">เลขที่ใบเสร็จ</p>
                <input type="text" class="h-10 rounded-lg outline-none" placeholder=""/>
                <p class="mb-0">จำนวนเงิน</p>
                <input type="text" class="h-10 rounded-lg outline-none" placeholder=""/>
                <p class="mb-0">เข้ารักษาวันที่</p>
                <input type="text" class="h-10 rounded-lg outline-none" placeholder=""/>
            </div>
            <div>
                <br>
                <a class="btn-next" href="\Preview">{{ msg }}</a>
            </div>
        </div>
    </div>
</template>

<script>
    //Your Javascript lives within the Script Tag
    export default {
        data: function() {
            return {
            msg: 'ส่งคำร้อง'
            };
        },
        
    };
</script>