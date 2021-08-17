<template>
    <div class="container space-contianer" align="center">
        <div class="container">
            <div class="row">
                <div class="col-12" align="center">
                    <h2 id="header2">ยืนยันจำนวนเงิน</h2>
                </div>
                <div class="col-12" align="center">
                    <div class="tab-user mt-4 mb-4">
                        <div class="card-body">
                            <div class="col-12">
                                <p class="title-claim-number" style="margin-top: -10px">เลขคำร้อง : {{claimNo}}</p>
                            </div>
                            <div class="col-12">
                                <p class="text-start" style="margin-bottom: -10px">บริษัท กลางฯ สามาระจ่ายเงินให้ท่านได้จำนวน {{money}} บาท เนื่องจาก {{reason}} </p>
                            </div>
                        </div>

                    </div>
                </div>


            </div>
            <div class="row mb-2">
                <div class="col-12">
                    <div class="form-check">
                        <input class="form-check-input" type="checkbox" v-model="acceptR" >
                        <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                            ข้าพเจ้ายืนยันที่จะรับจำนวนเงินที่แจ้งมา
                        </p>
                    </div>
                </div>
            </div>
            <div class="row" v-if="acceptR">
                <div id="app" class="container ">
                    <p class="mt-4">
                        กรุณากดขอรหัส OTP เพื่อรับรหัสยืนยันจำนวนเงิน
                    </p>
                    <div class="row mb-4">
                        <div class="col-7">
                            <b-form-input class="mb-3" type="text" placeholder="xxx-xxx-9898" disabled />
                        </div>
                        <div class="col-5">
                            <button class="btn-request-otp" type="button">ขอรหัส OTP</button>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <p class="space-title mt-4">
                                กรุณากรอกรหัส OTP ที่ส่งไปยัง SMS
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div style="display: flex; flex-direction: row; margin-left: 10px">
                                <v-otp-input ref="otpInput"
                                             input-classes="otp-input"
                                             separator="-"
                                             :num-inputs="6"
                                             :should-auto-focus="true"
                                             :is-input-num="true"
                                             @on-change="handleOnChange"
                                             @on-complete="handleOnComplete" />

                                <!--<button @click="handleClearInput()">Clear Input</button>-->
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6  pt-2">
                            <p class=" pink-title fw-bold text-start "><ion-icon name="reload-outline" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>ขอรหัสอีกครั้ง</p>
                        </div>
                        <div class="col-6  pt-2">
                            <p class=" black-title fw-bold text-start"><ion-icon name="time-outline" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>00:20 น.</p>
                        </div>
                    </div>
                    <div>
                        <br>
                        <button class="btn-confirm-money" type="button" @click="showSwal">{{lblButton}}</button>
                    </div>
                </div>
            </div>
        </div>
        <b-modal id="modal-1" title="BootstrapVue">
            <p class="my-4">Hello from modal!</p>
        </b-modal>
    </div>
</template>

<style>
    .btn-confirm-money {
        background-color: var(--main-color);
        border: none;
        color: white;
        padding: 5px 20px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 15px;
    }
    .title-claim-number{
        font-weight:bold;
        font-size:15px;
        text-align:start;
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
                this.$swal({
                    icon: 'success',
                    text: 'บริษัทจะแจ้งวันที่โอนเงินให้ท่านทราบอีกครั้ง',
                    title: 'ยืนยันจำนวนเงินเรียบร้อย',
                    /*footer: '<a href="">Why do I have this issue?</a>'*/
                    confirmButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-size: 20px; font-weight: bold; border-radius: 4px;'>ตกลง",
                    confirmButtonColor: '#dad5e9',
                }).then((result) => {

                    if (result.isConfirmed) {
                        this.$router.push({ name: 'CheckStatus' })
                    } 
                });
            },
            handleOnComplete(value) {
                console.log('OTP completed: ', value);
            },
            handleOnChange(value) {
                console.log('OTP changed: ', value);
            },
            handleClearInput() {
                this.$refs.otpInput.clearInput();
            },
        }
        
        

    };
</script>
