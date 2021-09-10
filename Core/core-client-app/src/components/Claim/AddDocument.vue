<template>
    <div class="container" align="center">
        <h2 id="header2" class="mb-4">กรอกข้อมูล/แนบเอกสารเพิ่มเติม</h2>
        <div align="left">
            <p style="font-weight: bold">
                <ion-icon name="newspaper-outline" style="font-size: 20px; margin-bottom: -5px;"></ion-icon> เลขที่รับแจ้ง: {{ hosData.accNo }}<br>
                <ion-icon name="calendar-outline" style="font-size: 20px; margin-bottom: -5px;"></ion-icon> วันที่ยื่นคำร้อง: {{ hosData.stringRegDate }}<br>
            </p>
        </div>
        <div class="tab-user mb-3">
            <div class="row">
                <div class="col-3 px-0">
                    <img src="@/assets/kid.png" width="70">
                </div>
                <div class="col-9 text-start px-2">
                    <span>ชื่อ-สกุล: <span style="color: var(--main-color)">{{userData.prefix}}{{userData.fname}} {{userData.lname}}</span></span><br>
                    <span>เบอร์โทรศัพท์: <span style="color: var(--main-color)">0989525299</span></span><br>
                    <span>สถานะ: <span style="color: var(--main-color)">ผู้ประสบภัยจากรถ</span></span>
                </div>
            </div>
        </div>
        <div align="left">
            <p>มีความจำนงค์ในการติดต่อ บริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด เพื่อกระทำการ <span style="font-weight: bold"> ยื่นคำร้องขอรับเงินเบิกค่าเสียหายเบื้องต้น</span></p>
            <p style="font-weight: bold;">จำนวนเงิน 1,000 บาท</p>
            <p>
                จากกรมธรรม์ประกันภัยของบริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด กรณีการเกิดอุบัติเหตุของ<br />
                รถหมายเลขทะเบียน <span v-for="(car, index) in accData.car" :key="`car-${index}`" style="font-weight: bold">{{car}} </span><br />
                ในวันที่ <span style="font-weight: bold"> {{accData.stringAccDate}}</span>
            </p>
            <p style="margin-bottom: 0px">โดยมีเอกสารหลักฐานที่นำมาแสดง ประกอบด้วย</p>
            <vs-checkbox color="var(--main-color)" v-model="doc1">
                <template #icon>
                    <i class='ti ti-check'></i>
                </template>
                ใบเสร็จรับเงินค่ารักษาพยาบาล
            </vs-checkbox>
            <vs-checkbox color="var(--main-color)" v-model="doc2">
                <template #icon>
                    <i class='ti ti-check'></i>
                </template>
                ใบรับรองแพทย์
            </vs-checkbox>
            <vs-checkbox color="var(--main-color)" v-model="doc3">
                <template #icon>
                    <i class='ti ti-check'></i>
                </template>
                สำเนาบันทึกประจำวันของพนักงานสอบสวน
            </vs-checkbox>
            <vs-checkbox color="var(--main-color)" v-model="doc4">
                <template #icon>
                    <i class='ti ti-check'></i>
                </template>
                หน้าบัญชีธนาคาร
            </vs-checkbox>
        </div>
        <div align="left" class="mt-4">
            <label>เอกสารประกอบคำร้องกรณีเบิกค่าสูญเสียอวัยวะ/ทุพพลภาพ</label>
        </div>
        <!-- <div class="box-container">
        <p class="mb-0">สำเนาบัตรประจำตัวประชาชน</p>
        <div>
            <file-pond name="idCardFile"
                        ref="pond"
                        label-idle="กดที่นี่เพื่ออัพโหลดสำเนาบัตรประจำตัวประชาชน"
                        credits="null"
                        v-bind:allow-multiple="false"
                        v-bind:allowFileEncode="true"
                        accepted-file-types="image/jpeg, image/png"
                        v-bind:files="idCardFile"
                        v-on:addfile="onAddIdCardFile" />
        </div>
    </div>
    <br>-->
        <div class="box-container">
            <label class="px-2">อาการบาดเจ็บ</label>
            <b-form-input class="mt-0 mb-2" v-model="injuri" placeholder=""></b-form-input>
            <label class="px-2">ประเภทผู้ป่วย</label><br />
            <div style="float: left; margin-top: 3px;">
                <vs-radio v-model="typePatient" val="ผู้ป่วยใน" style="float: left">
                    ผู้ป่วยใน (นอนพักรักษาตัวในโรงพยาบาลตั้งแต่ 6 ชั่วโมงขึ้นไป)
                </vs-radio>
                <vs-radio v-model="typePatient" val="ผู้ป่วยนอก" class="mt-1" style="float: left">
                    ผู้ป่วยนอก (ไม่ได้นอนพักรักษาตัวในโรงพยาบาล)
                </vs-radio>

            </div>
            <br /><br /><br /><br />
        </div>
        <br />
        <div class="box-container mt-2" v-for="(input, index) in bills" :key="`Bill-${index}`">
            <div class="input wrapper flex items-center">
                <p class="px-2 mb-0">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                <!--<input type="file" @change="onFileChange">-->
                <file-pond credits="null"
                           label-idle="กดที่นี่เพื่ออัพโหลดใบเสร็จค่ารักษา"
                           v-bind:allow-multiple="false"
                           v-bind:allowFileEncode="true"
                           accepted-file-types="image/jpeg, image/png" />
                <vs-dialog width="550px" not-center v-model="modalHospital">
                    <template #header>
                        <h4 class="not-margin">
                            เลือกโรงพยาบาล
                        </h4>
                    </template>
                    <div class="con-content" align="left">
                        <div class="d-block text-left">
                            <div class="mb-2">
                                <label class="px-2">จังหวัด</label>
                                <select name="category" id="category" v-model="selectChangwat" @change="onChangwatChange">
                                    <option v-for="(category, index) in changwats" :value="category.changwatshortname" :key="index" style="font-size: 12px; line-height: 0px">
                                        {{ category.changwatname }}
                                    </option>
                                </select>
                            </div>
                            <div class="mb-2" v-show="divHospitalModal">
                                <label class="px-2">โรงพยาบาล</label>
                                <select name="item" id="item" v-model="mockHospital">
                                    <option v-for="(item, index) in filteredItems" :value="item.HOSPITALNAME " :key="index">
                                        {{ item.HOSPITALNAME  }}
                                    </option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <template #footer>
                        <div class="footer-dialog">
                            <vs-button block @click="submitModalHospital(index)">
                                ยืนยัน
                            </vs-button>

                        </div>
                    </template>
                </vs-dialog>
                <label class="px-2">โรงพยาบาล</label>
                <b-form-input class="mt-0 mb-2" v-model="input.selectHospital" type="text" @click="modalHospital=!modalHospital" />
                <label class="px-2">เลขที่ใบเสร็จ</label>
                <b-form-input class="mt-0 mb-2" v-model="input.bill_no" type="text" placeholder="" />
                <label class="px-2">จำนวนเงิน</label>
                <b-form-input class="mt-0 mb-2" v-model="input.money" type="number" placeholder="" @change="calMoney" />
                <label class="px-2">วันที่เข้ารักษา</label>
                <b-form-datepicker v-model="input.hospitalized_date"
                                   selected-variant="primary"
                                   label-selected=""
                                   label-no-date-selected=""
                                   :close-button="true"
                                   :label-help="null"
                                   label-close-button="ปิด"
                                   :today-button="true"
                                   label-today-button="เลือกวันปัจจุบัน"
                                   :date-format-options="{ year: 'numeric', month: 'numeric', day: 'numeric' }"
                                   size="sm"
                                   class="mt-0 mb-2 " locale="th" placeholder=""></b-form-datepicker>
                <div v-if="typePatient==='ผู้ป่วยใน'">
                    <label class="px-2">วันที่ออกจากโรงพยาบาล</label>
                    <b-form-datepicker v-model="input.out_hospital_date"
                                       selected-variant="primary"
                                       label-selected=""
                                       label-no-date-selected=""
                                       :close-button="true"
                                       :label-help="null"
                                       label-close-button="ปิด"
                                       :today-button="true"
                                       label-today-button="เลือกวันปัจจุบัน"
                                       :date-format-options="{ year: 'numeric', month: 'numeric', day: 'numeric' }"
                                       size="sm"
                                       class="mt-0 mb-2 " locale="th" placeholder=""></b-form-datepicker>
                </div>

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
                     class="ml-2 cursor-pointer mb-2"
                     style="margin-top: -10px;">
                    <path fill="none" d="M0 0h24v24H0z" />
                    <path fill="#EC4899" d="M12 22C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10-4.477 10-10 10zm0-2a8 8 0 1 0 0-16 8 8 0 0 0 0 16zm0-9.414l2.828-2.829 1.415 1.415L13.414 12l2.829 2.828-1.415 1.415L12 13.414l-2.828 2.829-1.415-1.415L10.586 12 7.757 9.172l1.415-1.415L12 10.586z" />
                </svg>

            </div>
        </div>

        <br>
        <p class="p_right">รวมจำนวนเงินที่ขอเบิก: {{ total_amount }} บาท</p>
        <br>
        <div align="left">
            <label>บัญชีรับเงิน</label>
        </div>
        <div class="box-container">
            <p class="mb-0">หน้าบัญชีธนาคาร</p>
            <div class="form-group ">
                <file-pond name="bankFile"
                           ref="pond"
                           credits="null"
                           label-idle="กดที่นี่เพื่ออัพโหลดรูปบัญชีธนาคาร"
                           v-bind:allow-multiple="false"
                           accepted-file-types="image/jpeg, image/png"
                           v-bind:files="bankFile"
                           v-on:addfile="onAddBankAccountFile" />
            </div>
        </div>
        <br /><br />
        <button class="btn-confirm-money" type="button" @click="showSwal">ส่งเอกสารเพิ่มเติม</button>
        <br><br><br>
    </div>
</template>

<script>
    import axios from 'axios'
    //Your Javascript lives within the Script Tag
    export default {
        name: "AddDocument",
        data() {
            return {
                msg: 'ดำเนินการต่อ',
                idCardFile: null,
                idCardFileDisplay: { file: null, filename: "", base64: "" },
                // ---Bill
                injuri: '',
                typePatient: 0,
                typeContect: 1,
                bills: [{ billNo: 1, bill_no: "", selectHospital: '', money: "", hospitalized_date: "", out_hospital_date: ""/* file: null, BillfileShow: "", filename: "" */ }],
                total_amount: 0,
                // --BookBank
                inputBank: { accountName: '', accountNumber: '', accountBankName: '' },
                bank: '',
                phoneNumbers: [{ phone: "" }],
                image: '',
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                formType: this.$route.params.type,
                // bankData: this.$store.state.bankStateData,
                billsFile: null,
                bankFile: null,
                bankFileDisplay: { file: null, filename: "", base64: "" },
                // ----Dialog
                active: false,
                dialogHospital: false,
                // Radio in Dialog
                picked: 1,
                //----Get Bank Name
                bankNames: [],
                selectBank: {
                    bankName: ''
                },
                //----Get Hospital Name
                hospitals: [],
                //selectHospital: '',
                //----Get Changwat Name
                changwats: [],
                selectChangwat: '',
                modalHospital: false,
                mockHospital: '',
                divHospitalModal: false,
                // HosApp
                hosData: this.$store.getters.hosAppGetter(this.$route.params.id),
                // Document Checkbox
                doc1: true,
                doc2: false,
                doc3: false,
                doc4: true,

              
            };
        },
        methods: {
            onAddIdCardFile: function (error, file) {
                this.idCardFileDisplay.file = file
                this.idCardFileDisplay.filename = file.filename
                this.idCardFileDisplay.base64 = file.getFileEncodeDataURL()
            },
            getBankNames() {
                console.log('getBankNames');
                var url = '/api/Master/Bank';
                axios.get(url)
                    .then((response) => {
                        this.bankNames = response.data;
                        console.log(response.data);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getHospitalNames() {
                console.log('getHospitalNames');
                var url = "https://ts2thairscapi.rvpeservice.com/3PAccidentAPI/api/Utility/Hospital";
                axios.post(url)
                    .then((response) => {
                        this.hospitals = response.data.data;
                        console.log(this.hospitals);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getChangwatNames() {
                console.log('getChangwatNames');
                var url = '/api/Master/Changwat';
                axios.get(url)
                    .then((response) => {
                        this.changwats = response.data;
                        console.log(response.data);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            onAddBankAccountFile: function (error, file) {
                this.bankFileDisplay.file = file
                this.bankFileDisplay.filename = file.filename
                this.bankFileDisplay.base64 = file.getFileEncodeDataURL()
            },
            processFilePageOne() {
                for (let i = 0; i < this.bills.length; i++) {
                    this.bills[i].BillfileShow = this.bills[i].file[0].getFileEncodeDataURL()
                    this.bills[i].filename = this.bills[i].file[0].filename
                }
                return true;
            },
            //getIt: function () {
            //    console.log(this.$refs.pond.getFiles());
            //},
            onChangwatChange() {

                this.divHospitalModal = true;

            },
            submitModalHospital(index) {
                this.bills[index].selectHospital = this.mockHospital
                this.modalHospital = false
                this.selectChangwat = 0;
                this.mockHospital = 0;
                this.divHospitalModal = false;
            },
            submitModalSaysoHospital() {
                this.saysoHospital = this.mockSaysoHospital
                this.modalSaysoHospital = false
                this.selectChangwat = 0;
                this.mockSaysoHospital = 0;
                this.divHospitalModal = false;
            },
            calMoney() {
                let sum = 0;
                for (let i = 0; i < this.bills.length; i++) {

                    sum = sum + parseInt(this.bills[i].money)
                }
                this.total_amount = sum

            },
            addField(value, fieldType) {
                var index = this.bills.length + 1
                fieldType.push({ billNo: index, bill_no: "", selectHospital: '', money: "", hospitalized_date: "", out_hospital_date: ""/*file: null, BillfileShow: "", filename: ""*/ });
                this.calMoney()
                console.log(this.bills)
            },
            removeField(index, fieldType) {
                //type.splice(index, 1);
                fieldType.splice(index, 1);
                this.calMoney()
                console.log(this.bills)
            },
            onFileChange(event) {
                var files = event.target.files || event.dataTransfer.files;
                if (!files.length)
                    return;
                console.log(files[0]);
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
            previewImage: function (event) {
                var input = event.target;
                if (input.files) {
                    var reader = new FileReader();
                    reader.onload = (e) => {
                        this.preview = e.target.result;
                        console.log(this.preview)
                    }
                    this.image = input.files[0];
                    reader.readAsDataURL(input.files[0]);
                }
            },
            showSwal() {
                this.$swal({
                    icon: 'success',
                    //text: 'ท่านสามารถติดตามผลดำเนินการได้ที่เมนูติดตามสถานะ',
                    title: 'ส่งเอกสารเพิ่มเติมแล้ว',
                    showCancelButton: false,
                    showDenyButton: false,
                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                    confirmButtonColor: '#5c2e91',
                    willClose: () => {
                        this.$router.push({ name: 'CheckStatus', params: { id: this.accData.stringAccNo } })
                    }
                })
            },
        },
        async mounted() {
            this.getBankNames();
            this.getHospitalNames();
            this.getChangwatNames();
            this.selectChangwat = 0;
            this.selectHospital = 0;
            console.log('accData', this.accData);
            console.log('hosData', this.hosData);
        },

        computed: {
            filteredItems: function () {
                return this.hospitals.filter(function (el) {
                    return el.CHANGWATSHORTNAME === this.selectChangwat;
                }, this);
            }
        }
    };
</script>

<style>
    
</style>