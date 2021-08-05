<template>
    <form-wizard title="" subtitle="" color="#5c2e91" step-size="xs" style="margin-top: -35px">
        <tab-content title="สร้างคำร้อง" icon="ti ti-write"> 
        <div class="">
            <form align="left">
                <label>ลักษณะบาดเจ็บ</label>
                <input type="text" class="h-10 rounded-lg outline-none" placeholder=""/>
                <br><br>
                <label>เอกสารประกอบคำร้องกรณีเบิกค่ารักษาพยาบาลเบื้องต้น</label>
            
            </form>
            <div class="box-container">
                <p class="mb-0">สำเนาบัตรประจำตัวประชาชน</p>
                <form action="/action_page.php">
                    <input type="file" name="filename">
                    <!--<input type="submit"> -->
                </form>
            </div>
            <br>
            <div class="box-container">
                <form action="/action_page.php">
                    <div v-for="(input, index) in bills" :key="`Bill-${index}`" class="input wrapper flex items-center">
                        <div v-if="!image">
                            <p class="mb-0">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                            <input type="file" @change="onFileChange">
                        </div>
                        <div v-else>
                            <img :src="image" />
                            <button @click="removeImage">Remove image</button>
                        </div>
                        <br>
                        <p class="mb-0">โรงพยาบาล</p>
                        <input v-model="input.hospital" type="text" class="h-10 rounded-lg outline-none" placeholder=""/>
                        <p class="mb-0">เลขที่ใบเสร็จ</p>
                        <input v-model="input.billNo" type="text" class="h-10 rounded-lg outline-none" placeholder=""/>
                        <p class="mb-0">จำนวนเงิน</p>
                        <input v-model="input.money" type="text" class="h-10 rounded-lg outline-none" placeholder=""/>
                        <p class="mb-0">เข้ารักษาวันที่</p>
                        <input v-model="input.hospitalized_date" type="text" class="h-10 rounded-lg outline-none" placeholder=""/>
                        <br>
                            <!-- Add Svg Icon-->
                            <p style="color: green">
                                <svg @click="addField(input, bills)"
                                        xmlns="http://www.w3.org/2000/svg"
                                        viewBox="0 0 30 30"
                                        width="30"
                                        height="30"
                                        class="ml-2 cursor-pointer">
                                    <path fill="none" d="M0 0h24v24H0z" />
                                    <path fill="green" d="M11 11V7h2v4h4v2h-4v4h-2v-4H7v-2h4zm1 11C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10-4.477 10-10 10zm0-2a8 8 0 1 0 0-16 8 8 0 0 0 0 16z" />
                                </svg>เพิ่มใบเสร็จ
                            </p>
                            <!-- Remove Svg Icon-->
                            <svg v-show="bills.length > 1"
                                    @click="removeField(index, bills)"
                                    xmlns="http://www.w3.org/2000/svg"
                                    viewBox="0 0 30 30"
                                    width="30"
                                    height="30"
                                    class="ml-2 cursor-pointer"
                                    style="margin-top: -10px;">
                                <path fill="none" d="M0 0h24v24H0z" />
                                <path fill="#EC4899" d="M12 22C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10-4.477 10-10 10zm0-2a8 8 0 1 0 0-16 8 8 0 0 0 0 16zm0-9.414l2.828-2.829 1.415 1.415L13.414 12l2.829 2.828-1.415 1.415L12 13.414l-2.828 2.829-1.415-1.415L10.586 12 7.757 9.172l1.415-1.415L12 10.586z" />
                            </svg>
                           
                    </div>
                </form>
            </div>
            <br>
            <br>
            <p class="p_right">รวมจำนวนเงินที่ขอเบิก: {{ total_amount }} บาท</p>
        </div>
        </tab-content>
        <tab-content title="บัญชีรับเงิน" icon="ti ti-money">
            <div>
            <form>
                <div class="form-group ">
                <p for="my-file" align="left">ภาพถ่ายหน้าสมุดบัญชีธนาคาร</p>
                <input type="file" accept="image/*" @change="previewImage" class="form-control-file" id="my-file" style="margin-top: -10px">
                <div class="border p-2 mt-2">
                    <p align="left">ภาพถ่ายที่เลือก:</p>
                    <template v-if="preview">
                    <img :src="preview" class="img-fluid" style="width: 30%"/>
                    <!--<p class="mb-0">ชื่อไฟล์: {{ image.name }}</p>
                    <p class="mb-0">size: {{ image.size/1024 }}KB</p>-->
                    </template>
                </div>
                </div>
            </form>
            <div>
                <div class="box-container mt-3">
                    <p class="mb-0">ชื่อธนาคาร</p>
                    <input type="text" placeholder=""/>
                    <p class="mb-0">ชื่อบัญชีธนาคาร</p>
                    <input type="text" placeholder=""/>
                    <p class="mb-0">เลขบัญชีธนาคาร</p>
                    <input type="text" placeholder=""/>
                </div>
                <br>
            </div>
            </div>
        </tab-content>
        <tab-content title="ส่งคำร้อง" icon="ti ti-id-badge">
            <div align="left" style="width: 100%;">
                <ion-icon name="people-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>
                <label align="left">ข้อมูลผู้ประสบภัย</label>
            </div>
            <div class="box-container mb-3">
                <div class="row">
                    <div class="col-9">
                        <p class="mb-0">ชื่อ-สกุล</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-3">
                        <p class="mb-0">อายุ</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
                <div class="row">
                    <div class="col-4">
                        <p class="mb-0">บ้านเลขที่</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-2">
                        <p class="mb-0">หมู่</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-6">
                        <p class="mb-0">ซอย</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">ถนน</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-6">
                        <p class="mb-0">ตำบล/แขวง</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">อำเภอ</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-6">
                        <p class="mb-0">จังหวัด</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">รหัสไปรษณีย์</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-6">
                        <p class="mb-0">เบอร์โทรศัพท์</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
            </div>

            <!-- เหตุ -->
            <div align="left" style="width: 100%;">
                <ion-icon name="bicycle-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>
                <label align="left">ข้อมูลอุบัติเหตุ</label>
            </div>
            <div class="box-container mb-3">
                <div class="row">
                    <div class="col-5">
                        <p class="mb-0">วันที่เกิดเหตุ</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-7">
                        <p class="mb-0">ลักษณะเกิดเหตุ</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
                <p class="mb-0">สถานที่เกิดเหตุ</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">รถคันเอาประกันภัย หมายเลขทะเบียน</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">เลขตัวถัง</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">กรมธรรม์คุ้มครองภัยจากรถ เลขที่</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />

            </div>

            <!-- เอกสาร -->
            <div align="left" style="width: 100%;">
                <ion-icon name="reader-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>
                <label align="left">ข้อมูลเอกสารประกอบคำร้อง</label>
            </div>
            <div class="box-container mb-3">
                <p class="mb-0">ลักษณะบาดเจ็บ</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">ประเภทผู้ป่วย</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">สำเนาบัตรประจำตัวประชาชน</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">ชื่อโรงพยาบาล</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">เลขที่ใบเสร็จ</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-6">
                        <p class="mb-0">วันที่เข้ารักษา</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">จำนวนเงิน</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-6">
                        <p class="mb-0">รวมเงินที่ขอเบิก</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
            </div>
            <!-- บัญชี -->
            <div align="left" style="width: 100%;">
                <ion-icon name="card-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>
                <label align="left">ข้อมูลบัญชีรับเงิน</label>
            </div>
            <div class="box-container mb-3">
                <p class="mb-0">ชื่อธนาคาร</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">ชื่อบัญชีธนาคาร</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">เลขบัญชีธนาคาร</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
            </div>

            <div class="form-check">
                <input class="form-check-input" type="checkbox" v-model="acceptClaim">
                <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                    ข้าพเจ้าตรวจสอบและยืนยันข้อมูลทุกอย่างเป็นความจริง
                </p>
            </div>
        </tab-content>
    </form-wizard>
</template>

<script>
    import { FormWizard, TabContent } from 'vue-form-wizard'
    import 'vue-form-wizard/dist/vue-form-wizard.min.css'
    //component code
    
    //Your Javascript lives within the Script Tag
    export default {
        name: "Claim",
        components: {
            FormWizard,
            TabContent
        },
        data() {
            return {
                // ---Bill
                bills: [{ billNo: "", hospital: "", bill_no: "", money: "", hospitalized_date: "", choose_file: "" }],
                phoneNumbers: [{ phone: "" }],
                image: '',
                total_amount: 4200,
                preview: null,
                // ---Preview
                acceptClaim: false,
            };
        },
        methods: {
            //---Bill
            addField(value, fieldType) {
                fieldType.push({ value: "" });
            },
            removeField(index, fieldType) {
                //type.splice(index, 1);
                fieldType.splice(index, 1);
            },
            onFileChange(e) {
                var files = e.target.files || e.dataTransfer.files;
                if (!files.length)
                    return;
                this.createImage(files[0]);
            },
            createImage(file) {
                var Image = new Image();
                var reader = new FileReader();
                var vm = this;

                reader.onload = (e) => {
                    vm.image = e.target.result;
                };
                reader.readAsDataURL(file);
            },
            removeImage: function () {
                this.image = '';
            },
            // --- BookBank
             previewImage: function(event) {
                var input = event.target;
                if (input.files) {
                    var reader = new FileReader();
                    reader.onload = (e) => {
                    this.preview = e.target.result;
                    }
                    this.image=input.files[0];
                    reader.readAsDataURL(input.files[0]);
                }
            }
        },
    };
</script>

<style>
    
</style>