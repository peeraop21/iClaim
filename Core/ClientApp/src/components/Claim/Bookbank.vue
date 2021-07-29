<template>
    <dev class="container" align="center">
       <div class="stepper-wrapper">
            <div class="stepper-item completed">
                <div class="step-counter"><ion-icon name="receipt-outline" style="font-size: 20px; color: white"></ion-icon></div>
                <p style="color: #5c2e91">สร้างคำร้อง</p>
            </div>
            <div class="stepper-item completed">
                <div class="step-counter"><ion-icon name="card-outline" style="font-size: 20px; color: white"></ion-icon></div>
                <p style="color: #5c2e91">บัญชีรับเงิน</p>
            </div>
            <div class="stepper-item">
                <div class="step-counter"><ion-icon name="send-outline" style="font-size: 20px; color: white"></ion-icon></div>
                <p style="color: #bbb">ส่งคำร้อง</p>
            </div>
        </div>
        <h2 id="header2">บัญชีรับเงิน</h2>
        <div id="app" class="container">
            <form>
                <div class="form-group ">
                <p for="my-file" align="left">ภาพถ่ายหน้าสมุดบัญชีธนาคาร</p>
                <input type="file" accept="image/*" @change="previewImage" class="form-control-file" id="my-file" style="margin-top: -10px">
                <div class="border p-2 mt-2">
                    <p align="left">ภาพถ่ายที่เลือก:</p>
                    <template v-if="preview">
                    <img :src="preview" class="img-fluid" style="width: 30%"/>
                    <p class="mb-0">file name: {{ image.name }}</p>
                    <p class="mb-0">size: {{ image.size/1024 }}KB</p>
                    </template>
                </div>
                </div>
            </form>
            <div>
                <div class="box-container">
                    <p class="mb-0">ชื่อธนาคาร</p>
                    <label class="select" for="slct">
                        <select id="slct" required="required">
                            <option value="" disabled="disabled" selected="selected">เลือกธนาคาร</option>
                            <option value="#">ธนาคารไทยพาณิชย์</option>
                            <option value="#">ธนาคารกรุงเทพ</option>
                            <option value="#">ธนาคารกรุงไทย</option>
                        </select>
                        <svg>
                            <use xlink:href="#select-arrow-down"></use>
                        </svg>
                        </label>
                        <!-- SVG Sprites-->
                        <svg class="sprites">
                        <symbol id="select-arrow-down" viewbox="0 0 10 6">
                            <polyline points="1 1 5 5 9 1"></polyline>
                        </symbol>
                        </svg>
                    <p class="mb-0">ชื่อบัญชีธนาคาร</p>
                    <input type="text" placeholder=""/>
                    <p class="mb-0">เลขบัญชีธนาคาร</p>
                    <input type="text" placeholder=""/>
                </div>
                <br>
                <a class="btn-next" href="\Preview">{{ msg }}</a>
            </div>
            
            <!--<div class="w-100"></div>
            <div class="col-12 mt-3 text-center">
            Reset input file <button @click="reset">Clear All</button>
            </div>-->
            
        </div>
    </dev>
</template>

<style>
.select {
  position: relative;
  min-width: 200px;
}
.select svg {
  position: absolute;
  right: 12px;
  top: calc(50% - 6px);
  width: 10px;
  height: 6px;
  stroke-width: 2px;
  stroke: #5c2e91;
  fill: none;
  stroke-linecap: round;
  stroke-linejoin: round;
  pointer-events: none;
}
.select select {
  -webkit-appearance: none;
  padding: 3px 40px 3px 12px;
  width: 300px;
  border: 2px solid #ccc;
  border-radius: 5px;
  background: #fff;
  cursor: pointer;
  transition: all 150ms ease;
  margin-bottom: 10px;
}
.select select:required:invalid {
  color: black;
}
.select select option {
  color: black;
}
.select select option[value=""][disabled] {
  display: none;
}
.select select:focus {
  outline: none;
  border-color: black;
  box-shadow: black;
}
.select select:hover + svg {
  stroke: #07f;
}
.sprites {
  position: absolute;
  width: 0;
  height: 0;
  pointer-events: none;
  user-select: none;
}
</style>
<script>
    //Your Javascript lives within the Script Tag
    export default {
        data: function() {
            return {
            preview: null,
            image: null,
            preview_list: [],
            image_list: [],
            msg: 'ดำเนินการต่อ'
            };
        },
        methods: {
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
                },
                previewMultiImage: function(event) {
                var input = event.target;
                var count = input.files.length;
                var index = 0;
                if (input.files) {
                    while(count --) {
                    var reader = new FileReader();
                    reader.onload = (e) => {
                        this.preview_list.push(e.target.result);
                    }
                    this.image_list.push(input.files[index]);
                    reader.readAsDataURL(input.files[index]);
                    index ++;
                    }
                }
                },
                reset: function() {
                this.image = null;
                this.preview = null;
                this.image_list = [];
                this.preview_list = [];
            }
        },
    };
</script>