<template>
    <div>
        <vs-dialog width="550px" not-center v-model="active">
            <template #header>
                <h4 class="not-margin">
                    คำรับรอง
                </h4>
            </template>
            <div class="con-content" align="left">
                <div class="d-block text-left">
                    <p>
                        ข้าพเจ้าผู้ยื่นคำร้องขอในนามของผู้ประสบภัย ขอให้ค้ารับรองว่า.-
                        <br>
                        1. ข้าพเจ้าหรือประสบภัยยังไม่เคยรับหรือทำสัญญาว่าจะรับค่าเสียหายจากเจ้าของรถ หรือผู้หนึ่งผู้ใด
                        ใด และยังไม่เคยรับหรือยื่นขอรับค่าเสียหายเบื้องต้นจากกองทุนทดแทนผู้ประสบภัย
                    </p>
                    <p>
                        2. ข้าพเจ้าหรือผู้ประสบภัย
                    </p>
                    <div style="margin-top: -10px;">
                        <vs-radio color="var(--main-color)" v-model="picked" val="1" class="mb-1" style="float: left">
                            ไม่เคย&emsp;&emsp;
                        </vs-radio>
                        <vs-radio color="#7d33ff" v-model="picked" val="2" style="float: left">
                            เคย
                        </vs-radio>
                    </div>
                    <br />
                    <div class="mt-0" v-if="picked==='2'">
                        <br />
                        <label>จำนวนเงิน</label>
                        <vs-input color="var(--main-color)"
                                  v-model="value7"
                                  placeholder="จำนวนเงิน" />
                        <label>สถานพยาบาลชื่อ</label>
                        <vs-input color="var(--main-color)"
                                  v-model="value7"
                                  placeholder="สถานพยาบาล" />

                    </div>
                    <p class="mt-3">
                        3. เมื่อข้าพเจ้าได้รับค่าเสียหายเบื้องต้นจากบริษัทประกันภัยครบถ้วนตามจำนวนที่กฎหมายกำหนดแล้ว
                        ข้าพเจ้าขอสัญญาว่า ข้าพเจ้าจะไม่ไปเรียกร้องค่าเสียหายเบื้องต้นจากเจ้าของรถ หรือมอบอำนาจให้บุคคลอื่น
                        หรือสถานพยาบาลมารับค่าเสียหายเบื้องต้นจำนวนนี้ซ้ำอีก
                    </p>
                    <div class="row">
                        <div class="col-2" style="padding-right: 0px; width:13%;">
                            <vs-checkbox color="#7d33ff" v-model="acceptClaim"></vs-checkbox>
                        </div>
                        <div class="col-10 px-0">
                            <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                                ข้าพเจ้าขอรับรองว่าข้อมูลดังกล่าวข้างต้นเป็นจริงทุกประการ หากข้าพเจ้าผิดคำรับรอง
                                ข้าพเจ้ายินยอมรับผิดในความเสียหายที่เกิดขึ้นทั้งหมดแก่บริษัท
                            </p>
                        </div>
                    </div>
                    <div v-if="acceptClaim" class="mb-4" align="center">
                        <router-link class="btn-next" :to="{ name: 'ConfirmOTP', params: { id: accData.eaTmpId}}">ยืนยันส่งคำร้อง</router-link>
                    </div>
                </div>
            </div>
        </vs-dialog>
        <form-wizard title="" subtitle="" color="#5c2e91" step-size="xs" style="margin-top: -35px;" next-button-text="ดำเนินการต่อ" back-button-text="ย้อนกลับ" finish-button-text="ส่งคำร้อง" @on-complete="active=!active">
            <tab-content title="สร้างคำร้อง" icon="ti ti-write" :before-change="processFilePageOne">
                <div class="">
                    <div align="left">
                        <label>เอกสารประกอบคำร้องกรณีเบิกค่ารักษาพยาบาลเบื้องต้น</label>
                    </div>
                    <div class="box-container">
                        <p class="mb-0">สำเนาบัตรประจำตัวประชาชน</p>
                        <div>
                            <!--<input type="file" name="filename">-->
                            <file-pond name="idCardFile"
                                       ref="pond"
                                       label-idle="กดที่นี่เพื่ออัพโหลดสำเนาบัตรประชาชน"
                                       credits="null"
                                       v-bind:allow-multiple="false"
                                       v-bind:allowFileEncode="true"
                                       accepted-file-types="image/jpeg, image/png"
                                       v-bind:files="idCardFile"
                                       v-on:addfile="onAddIdCardFile" />
                            <!--<input type="submit"> -->
                        </div>
                    </div>
                    <br>
                    <div class="box-container">
                        <label class="px-2">ลักษณะบาดเจ็บ</label>
                        <b-form-input class="mt-0 mb-2" v-model="injuri" placeholder=""></b-form-input>

                        <div v-for="(input, index) in bills" :key="`Bill-${index}`" class="input wrapper flex items-center">
                            <p class="px-2 mb-0">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                            <!--<input type="file" @change="onFileChange">-->
                            <file-pond credits="null"
                                       label-idle="กดที่นี่เพื่ออัพโหลดใบเสร็จค่ารักษา"
                                       v-bind:allow-multiple="false"
                                       v-bind:allowFileEncode="true"
                                       accepted-file-types="image/jpeg, image/png"
                                       v-model="input.file" />

                            <label class="px-2">โรงพยาบาล</label>
                            <b-form-input class="mt-0 mb-2" v-model="input.hospital" type="text" placeholder="" />
                            <label class="px-2">เลขที่ใบเสร็จ</label>
                            <b-form-input class="mt-0 mb-2" v-model="input.bill_no" type="text" placeholder="" />
                            <label class="px-2">จำนวนเงิน</label>
                            <b-form-input class="mt-0 mb-2" v-model="input.money" type="number" placeholder="" @change="calMoney" />
                            <label class="px-2">เข้ารักษาวันที่</label>
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
                                               class="mt-0 mb-2 " locale="th" placeholder="เลือกวันที่เข้ารักษา"></b-form-datepicker>
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
                    <br>
                    <p class="p_right">รวมจำนวนเงินที่ขอเบิก: {{ total_amount }} บาท</p>
                </div>
            </tab-content>
            <!-- บัญชีรับเงิน -->
            <tab-content title="บัญชีรับเงิน" icon="ti ti-money">
                <div>
                    <p for="my-file" align="left" class="title-advice-menu mb-0">ภาพถ่ายหน้าสมุดบัญชีธนาคาร</p>
                    <div class="box-container mt-0">
                        <div class="form-group ">

                            <file-pond name="bankFile"
                                       ref="pond"
                                       credits="null"
                                       label-idle="กดที่นี่เพื่ออัพโหลดรูปบัญชีธนาคาร"
                                       v-bind:allow-multiple="false"
                                       accepted-file-types="image/jpeg, image/png"
                                       v-bind:files="bankFile"
                                       v-on:addfile="onAddBankAccountFile" />
                            <!--<input type="file" accept="image/*" @change="previewImage" class="form-control-file" id="my-file" style="margin-top: -10px">
                        <div class="border p-2 mt-2">
                            <p align="left">ภาพถ่ายที่เลือก:</p>
                            <template v-if="preview">
                                <img :src="preview" class="img-fluid" style="width: 30%" />-->
                            <!--<p class="mb-0">ชื่อไฟล์: {{ image.name }}</p>
                        <p class="mb-0">size: {{ image.size/1024 }}KB</p>-->
                            <!--</template>
                        </div>-->
                        </div>

                        <div>

                            <p class="mb-0 px-2">ชื่อธนาคาร</p>
                            <b-form-input class="mt-0 mb-2" v-model="bookbank" placeholder=""></b-form-input>
                            <p class="mb-0 px-2">ชื่อบัญชีธนาคาร</p>
                            <b-form-input class="mt-0 mb-2" v-model="accountName" placeholder=""></b-form-input>
                            <p class="mb-0 px-2">เลขบัญชีธนาคาร</p>
                            <b-form-input class="mt-0 mb-2" v-model="accountNumber" placeholder=""></b-form-input>
                        </div>
                        <br>
                    </div>
                </div>
                <br>
            </tab-content>
            <!-- ผู้ประสบภัย -->
            <tab-content title="ส่งคำร้อง" icon="ti ti-id-badge">
                <div align="left" style="width: 100%;">
                    <ion-icon name="people-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                    <label align="left" class="title-advice-menu mb-1">ข้อมูลผู้ประสบภัย</label>
                </div>
                <div class="box-container mb-3">
                    <div class="row">
                        <div class="col-9">
                            <p class="mb-0">ชื่อ-สกุล</p>
                            <div class="mt-0" v-if="userData.prefix != null">
                                <p class="mb-0" style="color: grey">{{userData.prefix}}{{userData.fname}} {{userData.lname}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.prefix === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-3">
                            <p class="mb-0">อายุ</p>
                            <div class="mt-0" v-if="bank!=''">
                                <p class="mb-0" style="color: grey">{{bank}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="bank===''">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-4">
                            <p class="mb-0">บ้านเลขที่</p>
                            <div class="mt-0" v-if="userData.homeHouseNo != null">
                                <p class="mb-0" style="color: grey">{{userData.homeHouseNo}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeHouseNo === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-2">
                            <p class="mb-0">หมู่</p>
                            <div class="mt-0" v-if="userData.homeHmo != null">
                                <p class="mb-0" style="color: grey">{{userData.homeHmo}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeHmo === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">ซอย</p>
                            <div class="mt-0" v-if="userData.homeSoi != null">
                                <p class="mb-0" style="color: grey">{{userData.homeSoi}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeSoi === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <p class="mb-0">ถนน</p>
                            <div class="mt-0" v-if="userData.homeRoad != null">
                                <p class="mb-0" style="color: grey">{{userData.homeRoad}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeRoad === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">ตำบล/แขวง</p>
                            <div class="mt-0" v-if="userData.homeTumbolId != null">
                                <p class="mb-0" style="color: grey">{{userData.homeTumbolId}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeTumbolId === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <p class="mb-0">อำเภอ</p>
                            <div class="mt-0" v-if="userData.homeCityId != null">
                                <p class="mb-0" style="color: grey">{{userData.homeCityId}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeCityId === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">จังหวัด</p>
                            <div class="mt-0" v-if="userData.homeProvinceId != null">
                                <p class="mb-0" style="color: grey">{{userData.homeProvinceId}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeProvinceId === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <p class="mb-0">รหัสไปรษณีย์</p>
                            <div class="mt-0" v-if="userData.homeZipcode != null">
                                <p class="mb-0" style="color: grey">{{userData.homeZipcode}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeZipcode === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">เบอร์โทรศัพท์</p>
                            <div class="mt-0" v-if="userData.mobileNo != null">
                                <p class="mb-0" style="color: grey">{{userData.mobileNo}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.mobileNo === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                </div>

                <!-- เหตุ -->
                <div align="left" style="width: 100%;">
                    <ion-icon name="bicycle-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                    <label align="left" class="title-advice-menu mb-1">ข้อมูลอุบัติเหตุ</label>
                </div>
                <div class="box-container mb-3">

                    <p class="mb-0">วันที่เกิดเหตุ</p>
                    <div class="mt-0" v-if="accData.eaAccDate != null">
                        <p class="mb-0" style="color: grey">{{accData.stringAccDate}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="accData.eaAccDate === null">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">ลักษณะเกิดเหตุ</p>
                    <div class="mt-0" v-if="bank!=''">
                        <p class="mb-0" style="color: grey">{{bank}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bank===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">สถานที่เกิดเหตุ</p>
                    <div class="mt-0" v-if="bank!=''">
                        <p class="mb-0" style="color: grey">{{bank}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bank===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">รถคันเอาประกันภัย หมายเลขทะเบียน</p>
                    <div class="mt-0" v-if="bank!=''">
                        <p class="mb-0" style="color: grey">{{bank}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bank===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">เลขตัวถัง</p>
                    <div class="mt-0" v-if="bank!=''">
                        <p class="mb-0" style="color: grey">{{bank}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bank===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">กรมธรรม์คุ้มครองภัยจากรถ เลขที่</p>
                    <div class="mt-0" v-if="bank!=''">
                        <p class="mb-0" style="color: grey">{{bank}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bank===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>

                </div>

                <!-- เอกสาร -->
                <div align="left" style="width: 100%;">
                    <ion-icon name="reader-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                    <label align="left" class="title-advice-menu mb-1">ข้อมูลเอกสารประกอบคำร้อง</label>
                </div>
                <div class="box-container mb-3">
                    <p class="mb-0">ลักษณะบาดเจ็บ</p>
                    <div class="mt-0" v-if="injuri!=''">
                        <p class="mb-0" style="color: grey">{{injuri}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="injuri===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">ประเภทผู้ป่วย</p>
                    <div class="mt-0" v-if="patientType!=''">
                        <p class="mb-0" style="color: grey">{{patientType}}</p>
                        <hr>
                    </div>
                    <div class="mt-0" v-else-if="patientType===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>

                    <p class="mb-0">สำเนาบัตรประจำตัวประชาชน</p>
                    <div v-if="idCardFileDisplay" align="center">
                        <img class="img-show" :src="idCardFileDisplay.base64" />
                        <br />
                        <label>{{idCardFileDisplay.filename}}</label>
                    </div>

                    <div class="card-bill" v-for="bill in bills" :key="bill.billNo">
                        <p class="mb-0">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                        <div v-if="bill.BillfileShow" align="center">
                            <img class="img-show" :src="bill.BillfileShow" />
                            <br />
                            <label>{{bill.filename}}</label>
                        </div>
                        <!--<div class="mt-0" v-if="bill.BillfileShow!=''">
                        <p class="mb-0" style="color: grey">{{bill.BillfileShow}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bill.BillfileShow===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>-->
                        <p class="mb-0">ชื่อโรงพยาบาล</p>
                        <div class="mt-0" v-if="bill.hospital!=''">
                            <p class="mb-0" style="color: grey">{{bill.hospital}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="bill.hospital===''">
                            <p class="mb-0" style="color: grey">-</p>
                            <hr class="mt-0">
                        </div>
                        <div class="row">
                            <div class="col-6">
                                <p class="mb-0">เลขที่ใบเสร็จ</p>
                                <div class="mt-0" v-if="bill.bill_no!=''">
                                    <p class="mb-0" style="color: grey">{{bill.bill_no}}</p>
                                    <hr class="mt-0">
                                </div>
                                <div class="mt-0" v-else-if="bill.bill_no===''">
                                    <p class="mb-0" style="color: grey">-</p>
                                    <hr class="mt-0">
                                </div>
                            </div>
                            <div class="col-6">
                                <p class="mb-0">วันที่เข้ารักษา</p>
                                <div class="mt-0" v-if="bill.hospitalized_date!=''">
                                    <p class="mb-0" style="color: grey">{{bill.hospitalized_date}}</p>
                                    <hr class="mt-0">
                                </div>
                                <div class="mt-0" v-else-if="bill.hospitalized_date===''">
                                    <p class="mb-0" style="color: grey">-</p>
                                    <hr class="mt-0">
                                </div>
                            </div>
                        </div>
                        <p class="mb-0">จำนวนเงิน</p>
                        <div class="mt-0" v-if="bill.money!=''">
                            <p class="mb-0" style="color: grey">{{bill.money}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="bill.money===''">
                            <p class="mb-0" style="color: grey">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>

                    <p class="mb-0">รวมเงินที่ขอเบิก</p>
                    <p class="mb-0" style="color: grey">{{total_amount}}</p>
                    <hr class="mt-0">
                </div>
                <!-- บัญชี -->
                <div align="left" style="width: 100%;">
                    <ion-icon name="card-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                    <label align="left" class="title-advice-menu mb-1">ข้อมูลบัญชีรับเงิน</label>
                </div>
                <div class="box-container mb-3">
                    <p class="mb-0">หน้าสมุดบัญชีธนาคาร</p>

                    <div v-if="bankFileDisplay" align="center">
                        <img class="img-show" :src="bankFileDisplay.base64" />
                        <br />
                        <label>{{bankFileDisplay.filename}}</label>
                    </div>
                    <!--<div class="mt-0" v-if="bankFileShow!=''">
                        <p class="mb-0" style="color: grey">{{image.name}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-if="bankFileShow === ''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>-->
                    <p class="mb-0">ชื่อธนาคาร</p>
                    <div class="mt-0" v-if="bank!=''">
                        <p class="mb-0" style="color: grey">{{bookbank}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bank===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>

                    <p class="mb-0">ชื่อบัญชีธนาคาร</p>
                    <div class="mt-0" v-if="accountName!=''">
                        <p class="mb-0" style="color: grey">{{accountName}}</p>
                        <hr class="mt-0">
                        <div class="row">
                            <div class="col-6">
                                <p class="mb-0">เลขที่ใบเสร็จ</p>
                                <div class="mt-0" v-if="bill.bill_no!=''">
                                    <p class="mb-0" style="color: grey">{{bill.bill_no}}</p>
                                    <hr class="mt-0">
                                </div>
                                <div class="mt-0" v-else-if="bill.bill_no===''">
                                    <p class="mb-0" style="color: grey">-</p>
                                    <hr class="mt-0">
                                </div>
                            </div>
                            <div class="col-6">
                                <p class="mb-0">วันที่เข้ารักษา</p>
                                <div class="mt-0" v-if="bill.hospitalized_date!=''">
                                    <p class="mb-0" style="color: grey">{{bill.hospitalized_date}}</p>
                                    <hr class="mt-0">
                                </div>
                                <div class="mt-0" v-else-if="bill.hospitalized_date===''">
                                    <p class="mb-0" style="color: grey">-</p>
                                    <hr class="mt-0">
                                </div>
                            </div>
                        </div>
                        <!--<div class="mt-0" v-else-if="accountName===''">
                            <p class="mb-0" style="color: grey">-</p>
                            <hr class="mt-0">
                        </div>-->
                        <p class="mb-0">เลขบัญชีธนาคาร</p>
                        <div class="mt-0" v-if="accountNumber!=''">
                            <p class="mb-0" style="color: grey">{{accountNumber}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="accountNumber===''">
                            <p class="mb-0" style="color: grey">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>

                    <!-- <div class="form-check">
                    <input class="form-check-input" type="checkbox" v-model="acceptClaim">
                    <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                        ข้าพเจ้าตรวจสอบและยืนยันข้อมูลทุกอย่างเป็นความจริง
                    </p>
                    </div> -->
                </div>
            </tab-content>

        </form-wizard>
    </div>
</template>

<script>
    import { FormWizard, TabContent } from 'vue-form-wizard'
    import 'vue-form-wizard/dist/vue-form-wizard.min.css'
    import vueFilePond from 'vue-filepond';

    // Import FilePond styles
    import 'filepond/dist/filepond.min.css'

    // Import FilePond plugins
    // Please note that you need to install these plugins separately

    // Import image preview plugin styles
    import 'filepond-plugin-image-preview/dist/filepond-plugin-image-preview.min.css';

    // Import image preview and file type validation plugins
    import FilePondPluginFileValidateType from 'filepond-plugin-file-validate-type';
    import FilePondPluginImagePreview from 'filepond-plugin-image-preview';
    import FilePondPluginFileEncode from 'filepond-plugin-file-encode';


    // Create component
    const FilePond = vueFilePond(FilePondPluginFileValidateType, FilePondPluginImagePreview, FilePondPluginFileEncode);


    //Your Javascript lives within the Script Tag
    export default {
        name: "Claim",
        components: {
            FormWizard,
            TabContent,
            FilePond
        },
        data() {
            return {
                // ---Bill
                injuri: '',
                patientType: '',
                bills: [{ billNo: 1, hospital: "", bill_no: "", money: "", hospitalized_date: "", file: null, BillfileShow: "", filename: "" }],
                total_amount: null,
                // --BookBank
                bank: '',
                bookbank: '',
                accountName: '',
                accountNumber: '',
                phoneNumbers: [{ phone: "" }],
                image: '',
                preview: null,
                // ---Preview
                acceptClaim: false,
                selected: 'first',
                options: [
                    { text: ' ไม่เคย', value: 'first' },
                    { text: ' เคย', value: 'second' },
                ],
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                idCardFile: null,
                billsFile: null,
                bankFile: null,
                idCardFileDisplay: { file: null, filename: "", base64: "" },
                bankFileDisplay: { file: null, filename: "", base64: "" },
                // ----Dialog
                active: false,
                // Radio in Dialog
                picked: 1,

            };
        },

        methods: {

            onAddIdCardFile: function (error, file) {
                this.idCardFileDisplay.file = file
                this.idCardFileDisplay.filename = file.filename
                this.idCardFileDisplay.base64 = file.getFileEncodeDataURL()
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
            calMoney() {
                let sum = 0;
                for (let i = 0; i < this.bills.length; i++) {

                    sum = sum + parseInt(this.bills[i].money)
                }
                this.total_amount = sum

            },

            addField(value, fieldType) {
                var index = this.bills.length + 1
                fieldType.push({ billNo: index, hospital: "", bill_no: "", money: "", hospitalized_date: "", file: null, BillfileShow: "", filename: "" });
                console.log(this.bills)
            },
            removeField(index, fieldType) {
                //type.splice(index, 1);
                fieldType.splice(index, 1);
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
            //---Alert
            onComplete: function () {
                alert("Thank you for your response! We will contact you soon.");
            },
        },
        mounted() {
            console.log(this.accData)
            console.log(this.userData)

        }
    };
</script>

<style>
    input[type=number] {
        width: 100%;
        padding: 0px 5px;
        margin: 0px 0px;
        box-sizing: border-box;
        border: 2px solid #bbbbbb;
        border-radius: 4px;
        margin-top: -5px;
        margin-bottom: 10px;
    }

    input[type=date] {
        width: 100%;
        padding: 0px 5px;
        margin: 0px 0px;
        box-sizing: border-box;
        border: 2px solid #bbbbbb;
        border-radius: 4px;
        margin-top: -5px;
        margin-bottom: 10px;
    }

    .b-form-btn-label-control.form-control {
        border: 2px solid #bbbbbb;
        border-radius: 4px;
    }

        .b-form-btn-label-control.form-control > .dropdown-menu {
            padding: 0.5rem;
            background-color: #f8f9fa;
        }

    .img-show {
        margin: 5px;
        width: 30%;
    }

    .card-file {
        border: 1px dashed gray;
        opacity: 0.80;
        border-radius: 10px;
        padding: 5px;
        margin-bottom: 10px;
    }

    .card-bill {
        border: 1px solid #cccccc;
        border-radius: 10px;
        padding: 10px;
        margin-bottom: 10px;
    }

    #bv-modal {
        font-family: "Mitr";
    }

    .d-block {
        font-size: 14px;
        line-height: 20px;
    }

    div.wizard-footer-left {
        float: left;
        background-color: wheat;
        color: red;
    }

    .box-container {
        background-color: white;
        border: none;
    }
    /* Input */
    .mt-0.mb-2.form-control {
        background-color: #f3f2ff;
        border: none;
        border-radius: 10px;
        padding: 5px 10px;
    }

        .mt-0.mb-2.form-control:hover {
            border: none;
            box-shadow: 1px 2px var(--main-color);
            transform: translateX(1px);
        }
</style>