<template>
    <div class="container space-contianer" align="center">
        <div class="container">
            <div class="row">
                <div class="col-12" align="center">
                    <h2 id="header2">ยืนยันจำนวนเงิน</h2>
                </div>
                <div class="col-12" align="center">
                    <div class="card card-confirm-money-detail">
                        <div class="card-body">
                            <div class="col-12">
                                <p class="title-claim-number">เลขคำร้อง : {{claimNo}}</p>
                            </div>
                            <div class="col-12">
                                <p class="text-start">บริษัท กลางฯ สามาระจ่ายเงินให้ท่านได้จำนวน {{money}} บาท เนื่องจาก {{reason}} </p>
                            </div>
                        </div>

                    </div>
                </div>


            </div>
            <div class="row">
                <div class="col-12">
                    <div class="form-check">
                        <input class="form-check-input" type="checkbox" v-model="acceptR"   >
                        <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                            ข้าพเจ้าตรวจสอบและยืนยันข้อมูลทุกอย่างเป็นความจริง
                        </p>
                    </div>
                </div>
                
            </div>
            <div class="row" v-if="acceptR">
                <div id="app" class="container ">
                    <p>
                        กรุณากดขอรหัส OTP เพื่อยืนยันการส่งคำร้อง
                    </p>
                    <div class="row">
                        <div class="col-12">
                            <div class="card card-tel">
                                <div class="position-icon">
                                    <ion-icon name="call-outline"></ion-icon>
                                </div>

                                <label class="lbl-tel">xxx-xxx-9898</label>
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
                            <p class=" pink-title fw-bold text-start "><ion-icon name="reload-outline"></ion-icon>&nbsp;ส่งรหัสยืนยันอีกครั้ง</p>
                        </div>
                        <div class="col-6  pt-2">
                            <p class="  black-title fw-bold text-start"><ion-icon name="time-outline"></ion-icon>&nbsp;00:20 น.</p>
                        </div>
                    </div>
                    <div>
                        <br>
                        <button class="btn-confirm-money" type="button" @click="showSwal">{{lblButton}}</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style>
    .btn-confirm-money {
        background-color: #5c2e91;
        border:none;
        width: 130px;
        color: white;
        padding: 3px 10px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 30px;
    }
    .title-claim-number{
        font-weight:bold;
        font-size:20px;
        text-align:start;
    }
    .card-confirm-money-detail {
        border: 1px medium #5c2e91;
        padding: 2% 0px;
        width: 100%;
        margin: 2rem 0rem;
        border-radius: 10px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
    }
</style>

<script>
    
    //Your Javascript lives within the Script Tag
    export default {
        name: "ConfirmMoney",
        data() {
            return {
                acceptR: false,
                lblButton: 'ยืนยันจำนวนเงิน',
                claimNo: 'xxx/xxxx',
                money: '1,000',
                reason: '......'
            }
        },
        methods: {
            showSwal() {
                this.reason = "ทดสอบ";
                this.$swal('ยืนยันจำนวนเงินสำเร็จ!',
                    'ท่านจะได้รับเงินภายใน x วันทำการ',
                    'success');
            }
        }
        
        

    };
</script>
