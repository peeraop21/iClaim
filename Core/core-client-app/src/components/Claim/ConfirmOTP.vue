<template>
    <div class="container" align="center">
       
        <h2 id="header2" class="">ยืนยันการส่งคำร้อง</h2>
        <div id="app" class="container mt-5">
            <p>
                กรุณากดขอรหัส OTP เพื่อยืนยันการส่งคำร้อง
            </p>
            <div class="row">
                <div class="col-md-12 mb-5">
                    <div class="card card-tel">
                        <div class="position-icon mt-2">
                            <ion-icon name="call-outline"></ion-icon>
                            <label class="lbl-tel">xxx-xxx-9898</label>     
                        </div>  
                    </div>                  
                    <button class="btn-request-otp" type="button">ขอรหัส OTP</button>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <p class="space-title mt-5">
                        กรุณากรอกรหัสยืนยันที่ส่งไปยัง SMS
                    </p>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <input class="input-number-otp text-center" ref="digit1" maxlength="1" min="0" max="9" />
                    <input class="input-number-otp text-center" ref="digit2" maxlength="1" min="0" max="9" />
                    <input class="input-number-otp text-center" ref="digit3" maxlength="1" min="0" max="9" />
                    <input class="input-number-otp text-center" ref="digit4" maxlength="1" min="0" max="9" />
                    <input class="input-number-otp text-center" ref="digit5" maxlength="1" min="0" max="9" />
                    <input class="input-number-otp text-center" ref="digit6" maxlength="1" min="0" max="9" />
                </div>
            </div>
            <div class="row">
                <div class="col-6  pt-2">
                    <p class=" pink-title fw-bold text-start "><ion-icon name="reload-outline" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>&nbsp;ส่งรหัสยืนยันอีกครั้ง</p>
                </div>
                <div class="col-6  pt-2">
                    <p class="  black-title fw-bold text-start"><ion-icon name="time-outline" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>&nbsp;00:20 น.</p>
                </div>
            </div>
            <div>
                <br>
                <router-link class="btn-next" to="/Accident" >{{ msg }}</router-link>
            </div>
        </div>
    </div>
</template>

<style>
    .input-tel-otp {
        border-radius: 10px;
        background-color: gainsboro;
        font-size: 15px;
        margin-right: 1rem;
        padding: 0rem 1rem;
        font-weight: bold;
        width:60%
    }
    .btn-request-otp {
        border-radius: 10px;
        padding: 10px 10px;
        background-color: #5c2e91;
        color: whitesmoke;
        border-style: none;
        font-size: 15px;
    }
    .input-number-otp {
        margin:1rem 3% 0rem 3%;
        max-width: 2rem;
        border-radius: 10px;
        font-size: 15px;    
        border-style:groove;
    }
    .pink-title {
        color: hotpink;
        font-size: 13px;
        font-weight:bold;
        margin: 0 5% 0 5%;       
    }
    .pink-title:hover {
        cursor:pointer;
    }
    .black-title {
        color: dimgray;
        font-size: 13px;
        padding: 0% 20%;
        float: right;
        margin: 0 3% 0 3%;
        font-weight: bold;
    }
    .lbl-tel {
        font-size: 13px;
        text-align:start;
        font-weight:bold;
        color: black;
        padding-left: 10px;
    }
    .card-tel{
        padding:0 1rem;
        border:1px solid;
        display:block;
        float:left;
        margin-left:2%;
        width:55%;
        height:100%;
        background-color:gainsboro;
        border-radius:10px;
    }
    .position-icon {
        float: left;
        padding-top: 0%;
        color:deeppink;
        font-size: 20px; 
        text-align: start;
    }
   
    
</style>

<script>
    //Your Javascript lives within the Script Tag
    export default {
        data: function() {
            return {
                msg: 'ยืนยันการส่งคำร้อง',
                accData: this.$store.getters.accGetter(this.$route.params.id),
            };
        },
        
    };
</script>
